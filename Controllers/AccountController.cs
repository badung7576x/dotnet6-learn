using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UserManagement.ViewModels;
using UserManagement.Models;
using UserManagement.Mail;

namespace UserManagement.Controllers;

public class AccountController : Controller
{
  private readonly UserManager<AppUser> _userManager;
  private readonly SignInManager<AppUser> _signInManager;
  private readonly ISendMailService _emailSender;

  public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ISendMailService emailSender)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _emailSender = emailSender;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Login()
  {
    LoginViewModel loginVM = new LoginViewModel();

    return View(loginVM);
  }

  [HttpGet]
  public async Task<IActionResult> Register(string? returnUrl = null)
  {
    RegisterViewModel registerVM = new RegisterViewModel();
    registerVM.ReturnUrl = returnUrl;

    return View(registerVM);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel loginVM, string? returnUrl = null)
  {
    if (_signInManager.IsSignedIn(User)) return RedirectToAction("Index", "Home");

    if (ModelState.IsValid)
    {
      IdentityUser user = await _userManager.FindByEmailAsync(loginVM.Email);

      if (user == null)
      {
        ModelState.AddModelError("", "Tài khoản không tồn tại!");
        return View(loginVM);
      }

      var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, true, lockoutOnFailure: true);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }

      if (result.IsLockedOut)
      {
        return RedirectToAction("Lockout", "Account");
      }
      else
      {

        ModelState.AddModelError("", "Email hoặc mật khẩu chưa chính xác, vui lòng thử lại!");
      }

    }
    return View(loginVM);
  }

  public async Task<IActionResult> Lockout()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(RegisterViewModel registerVM, string? returnUrl = null)
  {
    registerVM.ReturnUrl = returnUrl;
    returnUrl = returnUrl ?? Url.Content("~/");

    if (ModelState.IsValid)
    {
      var user = new AppUser { Email = registerVM.Email, UserName = registerVM.Email };
      var result = await _userManager.CreateAsync(user, registerVM.Password);
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(returnUrl);
      }
      ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
    }

    return View(registerVM);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Logout()
  {
    await _signInManager.SignOutAsync();

    return RedirectToAction("Index", "Home");
  }

  [HttpGet]
  public async Task<IActionResult> ForgotPassword()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
  {
    if (!ModelState.IsValid) return View(forgotPasswordVM);
    var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
    if (user == null) return RedirectToAction("Index", "Home");

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    
    var callbackUrl = Url.Action(action: "ResetPassword", controller: "Account", values: new { token }, protocol: "https");

    await _emailSender.SendEmailAsync(
        forgotPasswordVM.Email,
        "Đặt lại mật khẩu",
        $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>bấm vào đây</a>.");

    return RedirectToAction("ForgotPasswordConfirmation", "Account");
  }

  [HttpGet]
  public IActionResult ForgotPasswordConfirmation()
  {
    return View();
  }

  [HttpGet]
  public IActionResult ResetPasswordConfirmation()
  {
    return View();
  }

  [HttpGet]
  public IActionResult ResetPassword(string token = null)
  {
    Console.WriteLine(token);
    ResetPasswordViewModel resetPasswordVM = new ResetPasswordViewModel();
    resetPasswordVM.Code = token;

    return View(resetPasswordVM);
  }

  [HttpPost]
  public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
  {
    if (ModelState.IsValid)
    {
      var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
      if (user != null)
      {
        var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Code, resetPasswordVM.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("ResetPasswordConfirmation", "Account");
        } else {
          ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi, không đặt lại được mật khẩu");
        }
      } else {
        ModelState.AddModelError(string.Empty, "Không tìm thấy tài khoản");
      }
    }
    return View(resetPasswordVM);
  }
}