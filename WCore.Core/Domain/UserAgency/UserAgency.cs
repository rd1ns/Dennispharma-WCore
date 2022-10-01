using System;
using System.ComponentModel.DataAnnotations;

namespace WCore.Core.Domain.Users
{
    public class UserAgency : BaseEntity
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Deleted { get; set; }
        public bool IsActive { get; set; }
    }
}
