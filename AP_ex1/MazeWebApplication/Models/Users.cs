using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazeWebApplication.Models
{
    public class Users
    {
        [Key]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public int wins { get; set; }

        [Required]
        public int losses { get; set; }

        [Required]
        public DateTime firstSignedIn { get; set; } 
    }
}