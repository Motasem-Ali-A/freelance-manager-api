using System.ComponentModel.DataAnnotations;

namespace FreelanceManager.Core.DTOs.Auth;
public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

[   Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string BusinessName { get; set; } = string.Empty;

}