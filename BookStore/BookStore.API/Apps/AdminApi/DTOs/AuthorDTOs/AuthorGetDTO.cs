using System;

namespace BookStore.Api.DTOs.AuthorDTOs
{
    public class AuthorGetDTO
    {
        public int Id { get; set; }
        public string Fullname {get; set; }
        public int BornYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
