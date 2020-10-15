using System.Threading.Tasks;
using Shodypati.Models;

namespace Shodypati.DAL
{
    public interface IAccountAccessRepository<TEntity, in TPrimaryKey> where TEntity : class
    {
        //custom
        UserStatusInfo Register(RegisterViewModel model, ApplicationUserManager UserManager);
        UserStatusInfo RegisterPatient(RegisterPatient model, ApplicationUserManager UserManager);

        Task<UserStatusInfo> Login(LoginViewModel model, ApplicationSignInManager SignInManager,
            ApplicationUserManager UserManager);
    }
}