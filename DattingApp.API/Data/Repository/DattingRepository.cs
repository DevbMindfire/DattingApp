using System.Collections.Generic;
using System.Threading.Tasks;
using DattingApp.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DattingApp.API.Data.Repository
{
     public class DattingRepository : IDattingRepository
     {
         private readonly DataContext _context;
         public DattingRepository(DataContext context)
         {
               _context = context;
          }

          public void Add<T>(T entity) where T: class
          {
               _context.Add(entity);
          }

          public void Delete<T>(T entity) where T : class
          {
               _context.Remove(entity);
          }

          public async Task<Photo> GetMainPhotoFromUser(int userId)
          {
               return await _context.Photos.Where(use => use.UserId == userId).SingleOrDefaultAsync(p => p.IsMain);
          }

          public async Task<Photo> GetPhoto(int id)
          {
               return await _context.Photos.SingleOrDefaultAsync(use => use.Id ==id);
          }

          public async Task<User> GetUser(int id)
          {
               return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(use => use.Id == id);
          }

          public async Task<List<User>> GetUsers()
          {
               return await _context.Users.Include(p => p.Photos).ToListAsync();
          }

          public async Task<bool> SaveAll()
          {
               return await _context.SaveChangesAsync() > 0;
          }
     }
}