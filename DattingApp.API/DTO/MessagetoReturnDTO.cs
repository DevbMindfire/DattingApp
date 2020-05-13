using System;

namespace DattingApp.API.DTO
{
    public class MessagetoReturnDTO
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderKnowsAs { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientKnowsAs { get; set; } 
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public bool IsRead { get; set; }
        public DateTime? MessageSent { get; set; }
    }
}