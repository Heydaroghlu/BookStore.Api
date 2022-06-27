namespace BookStore.Api.Apps.AdminApi.DTOs.AccountDTOs
{
    public class AccountGetDTO
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public virtual string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }

    }
}
