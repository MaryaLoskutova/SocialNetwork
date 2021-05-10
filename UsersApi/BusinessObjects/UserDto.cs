using System.Runtime.Serialization;

namespace UsersApi.BusinessObjects
{
    [DataContract]
    public class UserDto
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        
        [DataMember(Name = "SubscribersCount")]
        public int SubscribersCount { get; set; }
    }
}