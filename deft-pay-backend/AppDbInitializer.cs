using deft_pay_backend.Models;
using deft_pay_backend.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace deft_pay_backend
{
    public class AppDbInitializer
    {
        public static void SeedDatabase(IConfiguration configuration,
                                        ILogger<AppDbInitializer> logger,
                                        RoleManager<ApplicationRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            SeedRolesAndUsers(userManager, roleManager, configuration, logger);
        }

        private static void SeedRolesAndUsers(UserManager<ApplicationUser> userManager,
                                     RoleManager<ApplicationRole> roleManager,
                                     IConfiguration configuration,
                                     ILogger<AppDbInitializer> logger)
        {
            logger.LogDebug("----At point one");

            var result1 = roleManager.FindByNameAsync(UserRoleConstants.USER).GetAwaiter().GetResult();

            if (result1 == null)
            {
                roleManager.CreateAsync(new ApplicationRole(UserRoleConstants.USER));
            }

            logger.LogDebug("----At point two");

            var result2 = roleManager.FindByNameAsync(UserRoleConstants.ADMIN).Result;

            if (result2 == null)
            {
                roleManager.CreateAsync(new ApplicationRole(UserRoleConstants.ADMIN));
            }

            string adminEmail = configuration["DefaultAdminEmail"];
            string adminPassword = configuration["DefaultAdminPassword"];

            // Setup admin user only if it is setup in appsettings
            if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
            {
                if (userManager.FindByEmailAsync(adminEmail).Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = adminEmail,
                        NormalizedUserName = adminEmail.ToUpper(),
                        Email = adminEmail,
                        NormalizedEmail = adminEmail.ToUpper(),
                        EmailConfirmed = true
                    };

                    try
                    {
                        IdentityResult result = userManager.CreateAsync(user, adminPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, UserRoleConstants.ADMIN).Wait();
                            logger.LogDebug("CREATED THE ADMIN USER");
                        }
                        else
                        {
                            logger.LogError("There was an error creating the user");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error creating admin user");
                    }
                }
            }
        }
    }
}
