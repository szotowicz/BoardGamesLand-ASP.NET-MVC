//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersPrivacyPolicy
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RuleId { get; set; }
        public int RuleLevel { get; set; }
    
        public virtual Users Users { get; set; }
        public virtual UsersPrivacyPolicyList UsersPrivacyPolicyList { get; set; }
    }
}
