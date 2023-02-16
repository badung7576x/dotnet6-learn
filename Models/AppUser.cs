using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class AppUser : IdentityUser
{
  // Custom User attribute here
}