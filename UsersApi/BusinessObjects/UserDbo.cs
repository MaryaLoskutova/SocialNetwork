using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace UsersApi.BusinessObjects
{
    [Table("User")]
    [Index(nameof(Name), IsUnique = true, Name = "Idx_UserName")]
    public class UserDbo
    {
        [Key] public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(64)")] public string Name { get; set; }

        [Column(TypeName = "bigint"), Required, ConcurrencyCheck, DefaultValue(0)]
        public long Timestamp { get; set; }
    }
}