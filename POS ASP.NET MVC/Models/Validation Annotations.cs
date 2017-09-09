using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace POSEntity
{

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
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
    }
    #endregion

    #region Address Validation Annotation


    #endregion

}