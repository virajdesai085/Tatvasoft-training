﻿using Helperland_projectContext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Helperland.Models
{
    public class Custom
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ServiceRequestId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string spFirstName { get; set; }
        public string spLastName { get; set; }
        public decimal Ratings { get; set; }
        public DateTime ServiceStartDate { get; set; } = DateTime.Now;
        public int? Status { get; set; }
        public int? ServiceProviderId { get; set; }
        public decimal TotalCost { get; set; }

    }
}
