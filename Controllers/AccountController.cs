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

  [HttpGet]
  public async Task<IActionResult> Register(string? returnUrl = null)
  {
    RegisterViewModel registerViewModel = new RegisterViewModel();
    registerViewModel.ReturnUrl = returnUrl;

    return View(registerViewModel);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string? returnUrl = null)
  {
    registerViewModel.ReturnUrl = returnUrl;
    returnUrl = returnUrl ?? Url.Content("~/");

    if (ModelState.IsValid)
    {
      var user = new AppUser { Email = registerViewModel.Email, UserName = registerViewModel.Email };
      var result = await _userManager.CreateAsync(user, registerViewModel.Password);
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, isPersistent: false);
        return LocalRedirect(returnUrl);
      }
      ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký!");
    }

    return View(registerViewModel);
  }
}