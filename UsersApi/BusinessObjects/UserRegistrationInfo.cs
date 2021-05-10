using System.ComponentModel.DataAnnotations;

namespace UsersApi.BusinessObjects
{
    public class UserRegistrationInfo
    {
        [Required]
        public string Name { get; set; }
    }
}