using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAttemptshwcore.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string HashedPassword { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Salt { get; set; }

        public int LoginAttempts { get; set; }

        public bool IsLocked { get; set; }
    }
}
