
using System.ComponentModel.DataAnnotations;
namespace UserManagement.ViewModels;

public class ResetPasswordViewModel
{
  [Required(ErrorMessage = "Email là trường bắt buộc")]
  [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
  [Display(Name = "Email")]
  public string Email { get; set; }

  [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
  [StringLength(100, ErrorMessage = "Mật khẩu phải có tối thiểu 6 ký tự", MinimumLength = 6)]
  [DataType(DataType.Password)]
  [Display(Name = "Mật khẩu")]
  public string Password { get; set; }

  [Required(ErrorMessage = "Mật khẩu (xác nhận) là trường bắt buộc")]
  [Display(Name = "Mật khẩu (xác nhận)")]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu (xác nhận) không giống nhau")]
  public string ConfirmPassword { get; set; }

  public string? Code { get; set; }
}