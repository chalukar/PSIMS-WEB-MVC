using PSIMS.Models.Locations;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace PSIMS.Models.QuotationModel
{
    [Table("Quotation")]
    public class Quotation
    {
        public Quotation()
        {
            bActive = true;
        }
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Quotation Order No")]
        public string QuotationCode { get; set; }

        [Required]
        [ForeignKey("QuotationCategory")]
        [Display(Name = "QuotationCategory")]
        public int QuotationCategoryID { get; set; }

        //public string OurRef { get; set; }
        [Display(Name = "Your Ref")]
        public string YourRef { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

      
        [Display(Name = "Discount")]
        public decimal? Discount { get; set; }

        [Display(Name = "VAT")]
        public decimal? Vat { get; set; }

        [Required]
        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Required]
        [Display(Name = "Quotation Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime QuoteDate { get; set; }

        [Required]
        [Display(Name = "Quotation Expire Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryQuote { get; set; }

        //[ForeignKey("Customer")]
        [Display(Name = "Customer Name")]
        public int CustomerID { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks{ get; set; }

        [Display(Name = "Header Note Line 01")]
        public string Header_Note_Line01 { get; set; }

        [Display(Name = "Header Note Line 02")]
        public string Header_Note_Line02 { get; set; }

        [Display(Name = "Special Offers")]
        public string Speical_Offers { get; set; }

        [Display(Name = "Terms & Conditions")]
        public string Terms_Conditions { get; set; }

        [Display(Name = "Status")]
        public bool bActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        public string UserID { get; set; }
        [ForeignKey("Location")]
        public int LocationID { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual QuotationCategory QuotationCategory { get; set; }
        public virtual ICollection<QuotationItem> QuotationItems{ get; set; }
    }
}