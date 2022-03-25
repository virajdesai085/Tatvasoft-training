using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Helperland_projectContext.Models
{
    public partial class UserAddress
    {
        public int AddressId { get; set; }

        [NotMapped]
        public int id { get; set; }
        public int UserId { get; set; }
        [StringLength(50)]
        public string AddressLine1 { get; set; }
        [StringLength(50)]
        public string AddressLine2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        public string State { get; set; }
        [StringLength(6, MinimumLength = 4 , ErrorMessage = "Please Enter Proper Postal Code.")]
        public string PostalCode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Mobile number")]
        public string Mobile { get; set; }
        [DataType(DataType.EmailAddress)]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        public virtual User User { get; set; }
    }
}
