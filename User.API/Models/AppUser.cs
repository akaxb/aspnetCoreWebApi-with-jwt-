using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using User.API.Models.ValidationAttributes;

namespace User.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [InvalidNameCheck("admin")]
        public string Name { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        [Phone]

        public string Phone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        public List<UserProperty> UserProperties { get; set; }
    }
}
