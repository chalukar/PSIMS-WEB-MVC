using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace PSIMS.Models.PurchaseModel
{
    [Table("Supplier")]
    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Supplier Name")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string SupplierName { get; set; }

        [Display(Name = "Office No")]
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid Office No")]
        [DataType(DataType.PhoneNumber)]
        public string TelPhoneNo { get; set; }

        [Display(Name = "Mobile No")]
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid Mobile No")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Display(Name = "Fax No")]
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid Fax No")]
        [DataType(DataType.PhoneNumber)]
        public string FaxNo { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "VAT No")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string Registation_No { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string Companyname { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string Address { get; set; }

        [Display(Name = "Address(Optional)")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string Address_line2 { get; set; }

        [Required]
        [Display(Name = "City")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string City { get; set; }

        [Display(Name = "State/Distric/province")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string State { get; set; }


        public int CountryID { get; set; }

        [Display(Name = "Status")]
        public Status? status { get; set; }  

        [Display(Name = "Description")]
        [StringLength(150, ErrorMessage = "Cannot accept more than 100 characters")]
        public string Description { get; set; }

        public string CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Last Update By")]
        public string LastUpdateBy { get; set; }

        [Display(Name = "Last Updated On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdateOn { get; set; }
        
        public virtual ICollection<Purchase> Purchases { get; set; }

        public virtual Country country { get; set; }

        public enum Status
        {

            Active,
            Inactice
   
        }


    }
}