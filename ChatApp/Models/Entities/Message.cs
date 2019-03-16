using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models.Entities
{
    public class Message : BaseEntity
    {
        [ForeignKey("Contact")]
        public int? ContactId { get; set; }
        public virtual User Contact { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public string MessageSend { get; set; }
        public string TimeSend { get; set; }
    }
}