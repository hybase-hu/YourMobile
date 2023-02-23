using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YourMobile.DataAccess.Data;
using YourMobile.Utility;

namespace YourMobile.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            AppDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception)
            {

            }

            if (! _roleManager.RoleExistsAsync(WebRules.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebRules.AdminRole)).GetAwaiter().GetResult();
               


                _userManager.CreateAsync(new IdentityUser
                {
                    UserName="admin@abby.com",
                    Email= "admin@abby.com",
                    EmailConfirmed=true,

                },"Steph880423").GetAwaiter().GetResult();

				IdentityUser admin = _db.Users.FirstOrDefault(u => u.Email == "admin@abby.com");
                _userManager.AddToRoleAsync(admin, WebRules.AdminRole).GetAwaiter().GetResult();



            }
        }
    }
}
