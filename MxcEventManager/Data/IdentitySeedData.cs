using Microsoft.AspNetCore.Identity;

namespace MxcEventManager.Data;

public static class IdentitySeedData
{
    private const string AdminUser = "Admin";

    private const string AdminPassword = "Secret123$";

    public static async Task IdentitySeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        IdentityUser? user = await userManager.FindByNameAsync(AdminUser);

        if (user is null)
        {
            user = new IdentityUser("Admin")
            {
                Email = "admin@example.com",
                PhoneNumber = "555-1234"
            };

            await userManager.CreateAsync(user, AdminPassword);
        }
    }
}
