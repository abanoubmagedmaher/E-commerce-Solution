using Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Street = "123 Admin St",
                        City = "AdminCity",
                        State = "AdminState",
                        ZipCode = "12345"
                    }

                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
         
    }
}
