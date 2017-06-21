using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazeWebApplication.Models
{
    
        public class User
        {
            [Key]
            public string UserName { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public int Wins { get; set; }

            [Required]
            public int Losses { get; set; }

            [Required]
            public string EmailAdress { get; set; }

            [Required]
            public DateTime FirstSignedIn { get; set; }

            
        }
}