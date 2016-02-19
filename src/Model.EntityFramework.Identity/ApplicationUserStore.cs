using Microsoft.AspNet.Identity.EntityFramework;

namespace Pally.Model.EntityFramework.Identity
{
    public class ApplicationUserStore<TUser>
    : ApplicationUserStore<TUser, ApplicationRole, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    where TUser : ApplicationUser
    {
        public ApplicationUserStore(IdentityDbContextBase<TUser> context)
            : base(context)
        {
        }
    }

    public class ApplicationUserStore<TUser, TRole, TUserLogin, TUserRole, TUserClaim>
        : UserStore<TUser, TRole, int, TUserLogin, TUserRole, TUserClaim>
        where TUser : IdentityUser<int, TUserLogin, TUserRole, TUserClaim>
        where TRole : IdentityRole<int, TUserRole>
        where TUserLogin : IdentityUserLogin<int>, new()
        where TUserRole : IdentityUserRole<int>, new()
        where TUserClaim : IdentityUserClaim<int>, new()
    {
        public ApplicationUserStore(IdentityDbContextBase<TUser, TRole, TUserLogin, TUserRole, TUserClaim> context)
            : base(context)
        {
        }
    }
}
