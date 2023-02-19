using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UserManagement.ViewModels;
using UserManagement.Models;

namespace UserManagement.Controllers;

public class AccountController : Controller
{
  private readonly UserManager<AppUser> _userManager;
  private readonly SignInManager<AppUser> _signInManager;

  public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
  {
    _userManager = userManager;
    _signInManager = signInManager;
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
    if(ModelState.IsValid)
    {
      var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, true, lockoutOnFailure: false);
      if(result.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }
      ModelState.AddModelError("", "Email hoặc mật khẩu chưa chính xác, vui lòng thử lại!");
    }
    return View(loginVM);
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
}