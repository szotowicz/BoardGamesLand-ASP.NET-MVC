using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplication.Models
{
    public class UserAccount
    {
        public int Key { get; set; }

        [Display(Name = "Nazwa konta:")]
        [Required(ErrorMessage = "Należy podać identyfikator")]
        public string Login { get; set; }

        [Display(Name = "Adres e-mail:")]
        [Required(ErrorMessage = "Należy podać adres email")]
        public string Email { get; set; }

        [Display(Name = "Hasło:")]
        [Required(ErrorMessage = "Należy podać hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Imię:")]
        [Required(ErrorMessage = "Należy podać imię")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko:")]
        [Required(ErrorMessage = "Należy podać nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Data urodzin:")]
        [Required(ErrorMessage = "Należy wybrać datę urodzin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Płeć:")]
        [Required(ErrorMessage = "Należy podać płeć")]
        public string Sex { get; set; }

        public string Avatar { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Należy zaakceptować regulamin")]
        public bool RegulationsAreAccepted { get; set; }
    }
}