using System.Threading.Tasks;
using DattingApp.API.Model;

namespace DattingApp.API.Data.Repository
{
    public interface IAuthRepository
    {
         Task<User> Register(User user ,string password);
         Task<User> Login(string UserName,string password);
         Task<bool> UserExists(string UserName);
         
    }
}