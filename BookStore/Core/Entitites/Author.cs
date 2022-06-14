namespace BookStore.Core.Entitites
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; }
        public int BornYear { get; set; }

    }
}
