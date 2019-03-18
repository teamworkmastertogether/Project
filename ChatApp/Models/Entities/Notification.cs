using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class Notification : BaseEntity
    {
        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public string TextNoti { get; set; }
        public string ClassIconName { get; set; }
        public bool NotificationState { get; set; }
    }
}