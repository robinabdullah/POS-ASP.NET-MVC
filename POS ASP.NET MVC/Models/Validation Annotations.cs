using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POSEntity
{
    #region Product Validation Annotation

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }

    public class ProductMetaData
    {
        [Key]
        public string ID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public Nullable<int> Quantity_Available { get; set; }
        [Required]
        public Nullable<int> Quantity_Sold { get; set; }
        [Required]
        public Nullable<int> Unit_Price { get; set; }
        [Required]
        public Nullable<int> Selling_Price { get; set; }
        [Required]
        public Nullable<System.DateTime> Date_Updated { get; set; }
        [Required]
        public string Unique_Barcode { get; set; }
    }
    #endregion

    #region Sale Validation Annotation

    [MetadataType(typeof(SaleMetaData))]
    public partial class Sale
    {

    }

    public class SaleMetaData
    {
        [Key]
        public int Invoice_ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

    }
    #endregion

    #region Barcode Validation Annotation

    [MetadataType(typeof(BarcodeMetaData))]
    public partial class Barcode
    {

    }

    public class BarcodeMetaData
    {
        [Key]
        public int Invoice_ID { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }

    }
    #endregion

    #region Gift Validation Annotation

    [MetadataType(typeof(GiftMetaData))]
    public partial class Gift
    {

    }

    public class GiftMetaData
    {
        [Key]
        public string Barcode { get; set; }

    }
    #endregion

    #region Address Validation Annotation
    [MetadataType(typeof(AddressMetaData))]
    public partial class Address
    {

    }

    public class AddressMetaData
    {
        [Required]
        public string City { get; set; }
        [Required]
        public string AddressLine { get; set; }
        [Required]
        public string Postal_Code { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Phone1 { get; set; }
    }
    #endregion

    

}