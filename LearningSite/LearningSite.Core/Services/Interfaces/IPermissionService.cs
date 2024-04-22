using LearningSite.DataLayer.Entities.Permissions;
using LearningSite.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSite.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

        List<Role> GetRoles();
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void EditRole(Role role);
        void DeleteRole(Role role);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);

        #endregion

        #region Permission
        List<Permission> GetAllPermission();
        void AddPermissionsToRoles(int roleId,List<int> permission);
        List<int> permissionsrole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permission);
        bool CheckPermission(int permissionId, string username);
        #endregion
    }
}
