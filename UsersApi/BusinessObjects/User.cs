using System.ComponentModel.DataAnnotations;

namespace UsersApi.BusinessObjects
{
    public class User
    {
        [Required]
        public string Name { get; set; }
    }
}