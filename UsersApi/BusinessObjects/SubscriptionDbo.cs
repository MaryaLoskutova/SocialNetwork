using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UsersApi.BusinessObjects
{
    [Table("Subscription")]
    [Index(nameof(SubscriberId), nameof(UserId), IsUnique = true, Name = "Idx_SubscriberId_UserId")]
    public class SubscriptionDbo
    {
        [Key] public Guid SubscriptionId { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid UserId { get; set; }
    }
}