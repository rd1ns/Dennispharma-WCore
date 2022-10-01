using WCore.Core.Domain.Users;

namespace WCore.Core.Domain.Roles
{
    public class UserRole : BaseEntity
    {
        public string CompanyName { get; set; }
        public string OperatingName { get; set; }
        public string RoleGroupName { get; set; }
        public string RoleName { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string UserName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsHidden { get; set; }
        public string ControllerGroup { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DataToken { get; set; }

        public int RoleGroupId { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
