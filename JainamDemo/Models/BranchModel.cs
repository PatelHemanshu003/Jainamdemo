using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JainamDemo.Models
{
    public class BranchModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Branch Code")]
        public string Branch_Code { get; set; }
        [Required]
        [Display(Name = "Branch Name")]
        public string Branch_Name { get; set; }
        [Required]
        [Display(Name = "Branch Address")]
        public string Branch_Addr { get; set; }
        [Required]
        [Display(Name = "Branch StartDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime Branch_InitDate { get; set; }
        
        [Required]
        [Display(Name = "Branch Logo")]
        public string Branch_Logo { get; set; }
        [Required]
        [Display(Name = "Branch Type")]
        public string Branch_Type { get; set; }
        [Required]
        [Display(Name = "Branch Database")]
        public string Branch_Db { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}