using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MazeWebApplication.Models
{

    /// <summary>
    /// Class representing a user.
    /// </summary>
    public class User
        {
            /// <summary>
            /// Gets or sets the name of the user.
            /// </summary>
            /// <value>
            /// The name of the user.
            /// </value>
            [Key]
            public string UserName { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            [Required]
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets the wins.
            /// </summary>
            /// <value>
            /// The wins.
            /// </value>
            [Required]
            public int Wins { get; set; }

            /// <summary>
            /// Gets or sets the losses.
            /// </summary>
            /// <value>
            /// The losses.
            /// </value>
            [Required]
            public int Losses { get; set; }

            /// <summary>
            /// Gets or sets the email adress.
            /// </summary>
            /// <value>
            /// The email adress.
            /// </value>
            [Required]
            public string EmailAdress { get; set; }

            /// <summary>
            /// Gets or sets the date signed in.
            /// </summary>
            /// <value>
            /// The date signed in.
            /// </value>
            [Required]
            public DateTime FirstSignedIn { get; set; }
        }
}