using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Context;
using LearningSite.DataLayer.Entities.Permissions;
using LearningSite.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningSite.Core.Services
{
    public class PermissionService : IPermissionService
    {
        private LearningSiteContext _context;

        public PermissionService(LearningSiteContext context)
        {
            _context = context;
        }
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //Delete All Roles User
            _context.UserRoles.Where(u => u.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));
            //Add New Roles User
            AddRolesToUser(rolesId, userId);

        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void EditRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            EditRole(role);
           
        }

        public List<Permission> GetAllPermission()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermissionsToRoles(int roleId, List<int> permission)
        {
            foreach (var p in permission)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = roleId
                });
            }
            _context.SaveChanges();
        }

        public List<int> permissionsrole(int roleId)
        {
            return _context.RolePermissions.Where(r => r.RoleId == roleId).Select(r => r.PermissionId).ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permission)
        {
            _context.RolePermissions.Where(p => p.RoleId == roleId).ToList().ForEach(p => _context.RolePermissions.Remove(p));
            AddPermissionsToRoles(roleId, permission);
        }

        public bool CheckPermission(int permissionId, string username)
        {
            int userid = _context.Users.Single(u => u.UserName == username).UserId;

            List<int> UserRoles = _context.UserRoles.Where(r => r.UserId == userid).Select(r => r.RoleId).ToList();
            if (!UserRoles.Any())
                return false;
            List<int> RolesPermission = _context.RolePermissions.Where(p => p.PermissionId == permissionId).Select(p=>p.RoleId).ToList();
            return RolesPermission.Any(p => UserRoles.Contains(p));
        }
    }
}
