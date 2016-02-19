using Microsoft.AspNet.Identity;

namespace Pally.Model.EntityFramework.Identity
{
    public class ApplicationUserManager<TUser> : UserManager<TUser, int>
        where TUser : ApplicationUser
    {
        public ApplicationUserManager(ApplicationUserStore<TUser> store)
            : base(store)
        {
        }
    }
}
