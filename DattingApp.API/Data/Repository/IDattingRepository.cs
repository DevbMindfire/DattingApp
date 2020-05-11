using System.Collections.Generic;
using System.Threading.Tasks;
using DattingApp.API.Helpers;
using DattingApp.API.Model;

namespace DattingApp.API.Data.Repository
{
    public interface IDattingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<User> GetUser(int id);
         Task<PageList<User>> GetUsers(UserParams userParams);
         Task<bool> SaveAll();
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoFromUser(int userId);
         Task<Like> GetLike(int userId , int recipientId);
    }
}