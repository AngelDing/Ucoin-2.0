
namespace Ucoin.Identity.DataObjects
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }       

        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }       
    }
}
