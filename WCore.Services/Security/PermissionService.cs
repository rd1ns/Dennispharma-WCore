using System;
using System.Collections.Generic;
using System.Text;
using WCore.Core.Domain.Security;
using WCore.Core.Domain.Users;

namespace WCore.Services.Security
{
    public class PermissionService : IPermissionService
    {
        public bool Authorize(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public bool Authorize(PermissionRecord permission, User user)
        {
            throw new NotImplementedException();
        }

        public bool Authorize(string permissionRecordSystemName)
        {
            throw new NotImplementedException();
        }

        public bool Authorize(string permissionRecordSystemName, User user)
        {
            throw new NotImplementedException();
        }

        public bool Authorize(string permissionRecordSystemName, int userRoleId)
        {
            throw new NotImplementedException();
        }

        public void DeletePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public void DeletePermissionRecordUserRoleMapping(int permissionId, int userRoleId)
        {
            throw new NotImplementedException();
        }

        public IList<PermissionRecord> GetAllPermissionRecords()
        {
            throw new NotImplementedException();
        }

        public IList<PermissionRecordUserRoleMapping> GetMappingByPermissionRecordId(int permissionId)
        {
            throw new NotImplementedException();
        }

        public PermissionRecord GetPermissionRecordById(int permissionId)
        {
            throw new NotImplementedException();
        }

        public PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            throw new NotImplementedException();
        }

        public void InsertPermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }

        public void InsertPermissionRecordUserRoleMapping(PermissionRecordUserRoleMapping permissionRecordUserRoleMapping)
        {
            throw new NotImplementedException();
        }

        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public void UpdatePermissionRecord(PermissionRecord permission)
        {
            throw new NotImplementedException();
        }
    }
}
