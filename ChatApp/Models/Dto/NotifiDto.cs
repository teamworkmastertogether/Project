using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Dto
{
    public class NotifiDto
    {
        public string NameOfUser { get; set; }
        public string SubjectName { get; set; }
        public string Avatar { get; set; }
        public int SubjectId { get; set; }
        public string TimeNotifi { get; set; }
        public int PostId { get; set; }
        public string TextNoti { get; set; }
        public string ClassIconName { get; set; }
        public bool NotificationState { get; set; }
        public int NotificationId { get; set; }
    }
}