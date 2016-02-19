using Microsoft.AspNet.Identity.EntityFramework;
using Pally.Model.EntityFramework.Core.Entity;

namespace Pally.Model.EntityFramework.Identity
{
    public class ApplicationUser : ApplicationUser<ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
    }

    public class ApplicationUser<TUserLogin, TUserRole, TUserClaim>
        : IdentityUser<int, TUserLogin, TUserRole, TUserClaim>, IEntity
        where TUserLogin : IdentityUserLogin<int>
        where TUserRole : IdentityUserRole<int>
        where TUserClaim : IdentityUserClaim<int>
    {
    }

    public class ApplicationRole : ApplicationRole<ApplicationUserRole>
    {
    }

    public class ApplicationRole<TUserRole> : IdentityRole<int, TUserRole>
        where TUserRole : ApplicationUserRole
    {
    }

    public class ApplicationUserRole : IdentityUserRole<int>
    {
    }

    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
    }

    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
    }
}
