using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; }
        public string Photo { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}