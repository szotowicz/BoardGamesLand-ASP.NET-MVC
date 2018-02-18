using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class UserProfile
    {
        public UserAccount UserWhomSomeoneVisits { get; set; }
        public List<PrivacyRules> UserPrivacyPolicy { get; set; }
        public List<int> UserFriendsIndex { get; set; }
    }
}