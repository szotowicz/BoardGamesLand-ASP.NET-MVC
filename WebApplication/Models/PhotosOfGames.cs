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
    
    public partial class PhotosOfGames
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
        public int TargetGameId { get; set; }
        public int UploaderId { get; set; }
    
        public virtual Games Games { get; set; }
        public virtual Users Users { get; set; }
    }
}
