namespace WCore.Core.Domain.Users
{
    public partial class UserAgencyAuthorization : BaseEntity
    {
        public bool IsRead { get; set; }
        public bool IsCreate { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }

        public int UserAgencyId { get; set; }
        public UserAgency UserAgency { get; set; }


    }
}
