using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Contact : BaseEntity
    {
        [ForeignKey("FromUser")]
        public int? FromUserId { get; set; }
        public virtual User FromUser { get; set; }
        [ForeignKey("ToUser")]
        public int? ToUserId { get; set; }
        public virtual User ToUser { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}