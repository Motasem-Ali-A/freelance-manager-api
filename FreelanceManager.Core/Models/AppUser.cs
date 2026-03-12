using Microsoft.AspNetCore.Identity;
namespace FreelanceManager.Core.Models
{
    public class AppUser : IdentityUser
    {
        private string FullName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
    }
}