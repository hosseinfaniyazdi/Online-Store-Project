using LearningSite.Core.Convertors;
using LearningSite.Core.DTOs;
using LearningSite.Core.Generator;
using LearningSite.Core.Security;
using LearningSite.Core.Services.Interfaces;
using LearningSite.DataLayer.Context;
using LearningSite.DataLayer.Entities.User;
using LearningSite.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LearningSite.Core.Services
{
    public class UserService : IUserService
    {
        private LearningSiteContext _context;

        public UserService(LearningSiteContext context)
        {
            _context = context;
        }


        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }

        public User LoginUser(LoginViewModel login)
        {
            string hashpassword = PasswordHelper.EncodePasswordMd5(login.Password);
            string email = FixedText.FixEmail(login.Email);
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hashpassword);
        }

        public bool ActiveAccount(string activecode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
            if (user == null || user.IsActive)
            {
                return false;
            }
            user.IsActive = true;
            user.ActiveCode = NameGenerator.GenerateUniqCode();
            _context.SaveChanges();
            return true;

        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByActiveCode(string activecode)
        {
            return _context.Users.SingleOrDefault(u => u.ActiveCode == activecode);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public InformationUserViewModel GetUserInformation(string username)
        {
            var user = GetUserByUsername(username);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = BalanceUserWallet(username);
            return information;
             
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == username);
        }

        public SideBarUserPanelViewModel GetSideBarUserPanelData(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new SideBarUserPanelViewModel()
            {
                UserName = u.UserName,
                ImageName = u.UserAvatar,
                RegisterDate = u.RegisterDate
            }).Single();
        }

        public EditProfileViewModel GetDataForEditProfileUser(string username)
        {
            return _context.Users.Where(u => u.UserName == username).Select(u => new EditProfileViewModel() {
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName

            }).Single();
        }

        public void EditProfile(string username , EditProfileViewModel edit)
        {
            if (edit.UserAvatar!=null)
            {
                string imagepath = "";
                if (edit.AvatarName!="Defult.jpg")
                {
                    imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", edit.AvatarName);
                    if (File.Exists(imagepath))
                    {
                        File.Delete(imagepath); 
                    }
                }
                edit.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(edit.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", edit.AvatarName);
                using (var stream=new FileStream(imagepath,FileMode.Create))
                {
                    edit.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByUsername(username);
            user.UserName = edit.UserName;
            user.Email = edit.Email;
            user.UserAvatar = edit.AvatarName;
            UpdateUser(user);
        }

        public bool CompareOldPassword(string oldpassword, string username)
        {
            string hasholdpass = PasswordHelper.EncodePasswordMd5(oldpassword);
            return _context.Users.Any(u => u.UserName == username && u.Password == hasholdpass);
        }

        public void ChangeUserPassword(string username, string newpassword)
        {
            var user = GetUserByUsername(username);
            user.Password = PasswordHelper.EncodePasswordMd5(newpassword);
            UpdateUser(user);
        }

        public int BalanceUserWallet(string username)
        {
            int userId = GetUserIdByUsername(username);
            var Enter = _context.Wallets.Where(u => u.UserId == userId && u.TypeId == 1 && u.IsPay).Select(u => u.Amount).ToList();
            var Exit = _context.Wallets.Where(u => u.UserId == userId && u.TypeId == 2).Select(u => u.Amount).ToList();
            return (Enter.Sum() - Exit.Sum());

        }

        public int GetUserIdByUsername(string username)
        {
            return _context.Users.Single(u => u.UserName == username).UserId;
        }

        public List<WalletViewModel> GetWalletUser(string username)
        {
            int userId = GetUserIdByUsername(username);
            return _context.Wallets.Where(w => w.IsPay == true && w.UserId == userId).Select(w => new WalletViewModel()
            {
                Amount = w.Amount,
                DateTime = w.CreateDate,
                Description = w.Description,
                Type = w.TypeId
            }).ToList();
        }

        public int ChargeWallet(string username, int amount,string description, bool ispay = false)
        {
            Wallet wallet = new Wallet
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = ispay,
                TypeId = 1,
                UserId = GetUserIdByUsername(username)
            };
          return AddWallet(wallet);

        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }

        public Wallet GetWalletByWalletId(int walletId)
        {
            return _context.Wallets.Find(walletId);
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        public UserForAdminViewModel GetUsers(int pageId = 1, string Filteremail = "", string Filterusername = "")
        {
            IQueryable<User> result = _context.Users;
            if (!string.IsNullOrEmpty(Filteremail))
            {
                result = result.Where(u => u.Email.Contains(Filteremail));
            }
            if (!string.IsNullOrEmpty(Filterusername))
            {
                result = result.Where(u => u.UserName.Contains(Filterusername));
            }
            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;
            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            return list;
        }

        public int AddUserFromAdmin(CreateUserViewModel user)
        {
            User usernew = new User();
            usernew.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            if (user.UserAvatar != null)
            {
                string imagepath = "";
                
                usernew.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", usernew.UserAvatar);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }
            usernew.ActiveCode = NameGenerator.GenerateUniqCode();
            usernew.Email = user.Email;
            usernew.IsActive = true;
            usernew.RegisterDate = DateTime.Now;
            usernew.UserName = user.UserName;
            return AddUser(usernew);
        }

        public EditUserViewModel GetUserForShowEditMode(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => new EditUserViewModel()
            {
                UserId = u.UserId,
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName,
                UserRoles = u.UserRoles.Select(r=>r.RoleId).ToList()
            }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }
            if (editUser.UserAvatar != null)
            {
                if (editUser.AvatarName != "Defult.jpg")
                {
                    // Delete Old Image
                    string deletpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletpath))
                    {
                        File.Delete(deletpath);
                    }
                }
                // Save New Image
                string imagepath = "";

                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public UserForAdminViewModel GetDeleteUsers(int pageId = 1, string Filteremail = "", string Filterusername = "")
        {
            IQueryable<User> result = _context.Users.IgnoreQueryFilters().Where(u=>u.IsDelete);
            if (!string.IsNullOrEmpty(Filteremail))
            {
                result = result.Where(u => u.Email.Contains(Filteremail));
            }
            if (!string.IsNullOrEmpty(Filterusername))
            {
                result = result.Where(u => u.UserName.Contains(Filterusername));
            }
            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;
            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();
            return list;
        }

        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;
            information.Wallet = BalanceUserWallet(user.UserName);
            return information;
        }

        public void RestoreUser(int userId)
        {
            User user = _context.Users.IgnoreQueryFilters().Single(u=>u.UserId==userId);
            user.IsDelete = false;
            UpdateUser(user);
        }
    }
}
