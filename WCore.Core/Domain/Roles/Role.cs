namespace WCore.Core.Domain.Roles
{
    public class Role : BaseEntity
    {
        public int RoleGroupId { get; set; }
        public virtual RoleGroup RoleGroup { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }

    }
    public class TempRole : BaseEntity
    {
        public int RoleGroupId { get; set; }
        public virtual TempRoleGroup RoleGroup { get; set; }
        public int MenuId { get; set; }
        public virtual Menu Menu { get; set; }

    }
}
