using System;

namespace DattingApp.API.Model
{
     public class Photo
     {
          public int Id { get; set; }
          public string Description { get; set; }
          public DateTime Added { get; set; }
          public string Url { get; set; }
          public bool IsMain { get; set; }
          public string PublicId { get; set; }
          public virtual User User { get; set; }
          public int UserId { get; set; }
     }
}