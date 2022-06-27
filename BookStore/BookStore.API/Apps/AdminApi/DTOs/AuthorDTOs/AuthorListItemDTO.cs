namespace BookStore.Api.DTOs.AuthorDTOs
{
    public class AuthorListItemDTO
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int BornYear { get; set; }
        public int BooksCount { get; set; }
    }
}
