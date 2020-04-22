using System;

namespace DattingApp.API.DTO
{
    public class PhotoDetailedDTO
    {
          public int Id { get; set; }
          public string Description { get; set; }
          public DateTime Added { get; set; }
          public string Url { get; set; }
          public bool IsMain { get; set; }
    }
}