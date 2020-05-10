using System.Collections.Generic;
using System.Threading.Tasks;
using DattingApp.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DattingApp.API.Helpers;
using System;

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

          public async Task<PageList<User>> GetUsers(UserParams userParams)
          {
               var user =  _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

               user = user.Where(use => use.Id != userParams.UserId);

               user = user.Where(use => use.Gender == userParams.Gender);

               if(userParams.MaxAge != 99 || userParams.MinAge != 18){

                    var maxDOB = DateTime.Now.AddYears(-userParams.MinAge);
                    var minDOB = DateTime.Now.AddYears(-userParams.MaxAge - 1);

                    user = user.Where(use => use.DateOfBirth >= minDOB && use.DateOfBirth <= maxDOB);

               }

               if(!string.IsNullOrEmpty(userParams.OrderBy)){

                    switch(userParams.OrderBy){
                         case "created":
                              user = user.OrderByDescending(u => u.Created);
                              break;
                         default:
                              user = user.OrderByDescending(u => u.LastActive);
                              break;
                    }

               }

               return await PageList<User>.CreateAsync(user,userParams.PageNumber,userParams.PageSize);

          }

          public async Task<bool> SaveAll()
          {
               return await _context.SaveChangesAsync() > 0;
          }
     }
}