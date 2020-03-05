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

            string defaultUserEmail = configuration["DefaultAdminEmail"];
            string defaultUserPassword = configuration["DefaultAdminPassword"];

            // Setup default user only if it is setup in appsettings
            if (!string.IsNullOrEmpty(defaultUserEmail) && !string.IsNullOrEmpty(defaultUserPassword))
            {
                if (userManager.FindByEmailAsync(defaultUserEmail).Result == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = defaultUserEmail,
                        NormalizedUserName = defaultUserEmail.ToUpper(),
                        Email = defaultUserEmail,
                        NormalizedEmail = defaultUserEmail.ToUpper(),
                        EmailConfirmed = true
                    };

                    try
                    {
                        IdentityResult result = userManager.CreateAsync(user, defaultUserPassword).Result;

                        if (result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user, UserRoleConstants.USER).Wait();
                            logger.LogDebug("CREATED THE Default USER");
                        }
                        else
                        {
                            logger.LogError("There was an error creating the user");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error creating default user");
                    }
                }
            }
        }
    }
}
