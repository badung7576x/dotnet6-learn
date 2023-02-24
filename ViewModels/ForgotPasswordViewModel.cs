
using System.ComponentModel.DataAnnotations;
namespace UserManagement.ViewModels;

public class ForgotPasswordViewModel
{
  [Required(ErrorMessage = "Email là trường bắt buộc")]
  [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
  [Display(Name = "Email")]
  public string Email { get; set; }
}