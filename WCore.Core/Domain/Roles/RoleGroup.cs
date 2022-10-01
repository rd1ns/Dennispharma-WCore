using System.ComponentModel.DataAnnotations;

namespace WCore.Core.Domain.Roles
{
    public class RoleGroup : BaseEntity
    {
        public string Name { get; set; }
        public RoleGroupType RoleGroupType { get; set; }
    }
    public class TempRoleGroup : BaseEntity
    {
        public string Name { get; set; }
        public RoleGroupType RoleGroupType { get; set; }
    }
    public enum RoleGroupType
    {
        Guest = 0,
        AgencyManager = 10,
        AgencyUser = 11,
        WCore = 99,
    }
}
