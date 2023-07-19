using PSIMS.Models.Finance;
using PSIMS.Models.QuotationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSIMS.Models.SalesModel
{
    [Table("Customer")]
    public class Customer
    {
        [Key, Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Display(Name="Customer Name")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string CustomerName { get; set; }

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

        [Display(Name = "Geo code:Latitude")]
        public string Latitude { get; set; }

        [Display(Name = "Geo code:Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "Last Update By")]
        public string LastUpdateBy { get; set; }

        [Display(Name = "Last Updated On")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdateOn { get; set; }

        [NotMapped]
        public HttpPostedFileBase File { get; set; }

        [NotMapped]
        public string ImageURL 
        { get 
            {
                return ID.ToString() + ".jpg";
            }  
        }

        [Display(Name = "Status")]
        public CstStatus? Status { get; set; }  

        public string CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateOn { get; set; }

        public bool CustNameIsActive { get; set;}

        public virtual ICollection<Sales> Sales { get; set; }

        public virtual ICollection<Quotation> Quotation { get; set; }
        public virtual ICollection<PaymentSettelmentMaster> PaymentSettelmentMaster { get; set; }

        public enum CstStatus
        {

            Active,
            Inactice

        }
    }
}