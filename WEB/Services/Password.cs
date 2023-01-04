using Microsoft.AspNetCore.Identity;
using System.Text;
using WEB.Models;

namespace WEB.Services
{
    public class Password
    {
        public static string GeneratePassword(UserManager<ApplicationUser> _userManager)
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            while (password.Length <= 9)
            {
                char c = (char)random.Next(65, 91);

                password.Append(c);
            }

            return password.ToString();
        }
    }
}
