using System.Collections.Generic;
using System.Threading.Tasks;
using DattingApp.API.Model;

namespace DattingApp.API.Data.Repository
{
    public interface IDattingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<User> GetUser(int id);
         Task<List<User>> GetUsers();
         Task<bool> SaveAll();

    }
}