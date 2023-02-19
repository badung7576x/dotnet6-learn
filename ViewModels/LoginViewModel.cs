
using System.ComponentModel.DataAnnotations;
namespace UserManagement.ViewModels;

public class LoginViewModel
{
  [Required(ErrorMessage = "Email là trường bắt buộc")]
  [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
  [Display(Name = "Email")]
  public string Email { get; set; }

  [Required(ErrorMessage = "Mật khẩu là trường bắt buộc")]
  [DataType(DataType.Password)]
  [Display(Name = "Mật khẩu")]
  public string Password { get; set; }

  public string? ReturnUrl { get; set; }
}