using Helperland_projectContext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public class UserManagement
    {
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ZipCode { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public int UserTypeId { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
