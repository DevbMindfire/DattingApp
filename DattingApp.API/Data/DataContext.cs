using DattingApp.API.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DattingApp.API.Data
{
    public class DataContext : IdentityDbContext<User,Role,int, IdentityUserClaim<int> 
                                ,UserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>, IdentityUserToken<int>>
    {

        public DataContext(DbContextOptions<DataContext> option):base(option){}

        public DbSet<Value> Values { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder){

            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>{
                userRole.HasKey(u => new {u.UserId , u.RoleId});
                
                userRole.HasOne(u => u.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(f => f.RoleId)
                        .IsRequired();

                 userRole.HasOne(u => u.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(f => f.UserId)
                        .IsRequired();
            });

            builder.Entity<Like>()
                .HasKey(k => new {k.LikeeId,k.LikerId});

            builder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)    
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)    
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);  
            
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MessagesRecieved)
                .OnDelete(DeleteBehavior.Restrict);    
        }
    }
}