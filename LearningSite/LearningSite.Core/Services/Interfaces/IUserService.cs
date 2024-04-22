using LearningSite.Core.DTOs;
using LearningSite.DataLayer.Entities.User;
using LearningSite.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSite.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserByEmail(string email);
        User GetUserByActiveCode(string activecode);
        User GetUserByUsername(string username);
        User GetUserById(int userId);
        void UpdateUser(User user);
        bool ActiveAccount(string activecode);
        int GetUserIdByUsername(string username);
        void DeleteUser(int userId);
        void RestoreUser(int userId);

        #region User Panel
        InformationUserViewModel GetUserInformation(string username);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarUserPanelViewModel GetSideBarUserPanelData(string username);
        EditProfileViewModel GetDataForEditProfileUser(string username);
        void EditProfile(string username , EditProfileViewModel edit);
        bool CompareOldPassword(string oldpassword, string username);
        void ChangeUserPassword(string username, string newpassword);
        #endregion

        #region Wallet
        int BalanceUserWallet(string username);
        List<WalletViewModel> GetWalletUser(string username);
        int ChargeWallet(string username, int amount, string description, bool ispay = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletId(int walletId);
        void UpdateWallet(Wallet wallet);
        #endregion

        #region Admin Panel
        UserForAdminViewModel GetUsers(int pageId = 1, string Filteremail = "", string Filterusername = "");
        UserForAdminViewModel GetDeleteUsers(int pageId = 1, string Filteremail = "", string Filterusername = "");
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        #endregion
    }
}
