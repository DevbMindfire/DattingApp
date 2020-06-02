using System.Collections.Generic;
using System.Linq;
using DattingApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DattingApp.API.Data
{
    public class Seed
    {
          public static void SeedUsers(UserManager<User> userManager){

              if(!userManager.Users.Any()){
                  var userData = System.IO.File.ReadAllText("Data/userSeeder.json");
                  var users = JsonConvert.DeserializeObject<List<User>>(userData);
                  foreach(var user in users){

                      userManager.CreateAsync(user , "password").Wait();

                  }
              }

          }
    }
}