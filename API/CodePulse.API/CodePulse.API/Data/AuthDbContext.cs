using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "E1A402B9-6760-46BC-8362-7CFDEDA9F162";
        var writerRoleId = "5F7CD2B2-09FB-4014-88B4-4F8656394929";
        
        // Create Reader and Writer Role (CREAMOS LOS ROLES EXISTENTES) 

        var roles = new List<IdentityRole>
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };

        // Seed the roles
        builder.Entity<IdentityRole>().HasData(roles);
        
        
        // Create an Admin User
        
        var adminUserId = "B3868FCE-DCA2-4813-BC29-1B2B6A6234F4";

        var admin = new IdentityUser()
        {
            Id = adminUserId,
            UserName = "admin@codepulse.com",
            Email = "admin@codepulse.com",
            NormalizedEmail = "admin@codepulse.com".ToUpper(),
            NormalizedUserName = "admin@codepulse.com".ToUpper()
        };


        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin2003!");
        builder.Entity<IdentityUser>().HasData(admin);

        // Give Roles to Admin

        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = adminUserId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminUserId,
                RoleId = writerRoleId
            }
        };
        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        
    }
}