using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace UsersApi.BusinessObjects
{
    [DataContract]
    public class UserRegistrationInfo
    {
        [Required]
        [DataMember(Name = "Name")]
        public string Name { get; set; }
    }
}