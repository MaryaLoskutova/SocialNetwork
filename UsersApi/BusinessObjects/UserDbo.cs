using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsersApi.DataBases
{
    [Table("User")]
    [Index(nameof(Name), IsUnique = true, Name = "Idx_UserName")]
    public class UserDbo
    {
        [Key]
        public Guid UserId { get; set; }
        
        [Column(TypeName = "nvarchar(64)")]
        public string Name { get; set; }
    }
}