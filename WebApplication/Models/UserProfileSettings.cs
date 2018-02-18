using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class UserProfileSettings
    {
        public UserAccount UserInformation { get; set; }
        public List<PrivacyRules> UserPrivacyPolicy { get; set; }
    }
}