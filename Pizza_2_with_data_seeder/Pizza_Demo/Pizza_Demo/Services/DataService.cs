using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Pizza_Demo.Models;

namespace Pizza_Demo.Services
{
    public class DataService
    {
        private readonly IServiceProvider _provider;


        public DataService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async void EnsureData(string adminPwd)
        {


            using (var serviceScope = _provider.GetService<IServiceScopeFactory>()
                .CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                var role = await roleManager.FindByNameAsync("Administrator");
                if (role == null)
                {
                    role = new IdentityRole() { Name = "Administrator" };
                    await roleManager.CreateAsync(role);
                }
                if (await userManager.FindByNameAsync("admin@admin.ch") == null)
                {
                    var user = new ApplicationUser() { UserName = "admin@admin.ch" };
                    await userManager.CreateAsync(user, adminPwd);
                    await userManager.AddToRoleAsync(user, "Administrator");
                }
            }
        }
    }
}
