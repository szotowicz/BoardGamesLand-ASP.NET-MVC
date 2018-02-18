using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Należy podać aktualne hasło")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Należy podać nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Należy powtórzyć nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPasswordCopy { get; set; }
    }
}