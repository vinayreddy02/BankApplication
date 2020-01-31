using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BankApplication.Services
{
    class UserServices
    {
        private List<User> Users = UtilServices.InitiateStaff();
        public List<User> GetAllUsers()
        {
            return Users;
        }
        public User CreateUser(User user)
        {
            try
            {
                user.ID = user.Name + DateTime.UtcNow.ToString("mmss");
                user.Password = user.Name+user.BankID;
                Users.Add(user);
                return user;
            }
            catch
            {
                return null;
            }
        }
        public User ResetPassword(string userID, string newPassword)
        {
            User user = GetUser(userID);
            try
            {
                user.Password = newPassword;
                return user;
            }
            catch
            {
                return null;
            }
        }
        public User GetUser(string ID)
        {
            try
            {
                User User = Users.FirstOrDefault(user => string.Equals(user.ID, ID));
                return User;
            }
            catch
            {
                return null;
            }
        }
        public bool RemoveUser(string ID)
        {
            try
            {
                Users = Users.FindAll(user => !string.Equals(user.ID, ID));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsStaffMember(string ID, string password, string bankID)
        {
            return Users.Any(user => string.Equals(user.ID, ID) && string.Equals(user.Password, password) && string.Equals(user.BankID, bankID) && (Role.staff == user.UserType));
        }
        public bool IsCustemer(string ID, string password, string bankID)
        {
            return Users.Any(user => string.Equals(user.ID, ID) && string.Equals(user.Password, password) && string.Equals(user.BankID, bankID));
        }
        public bool IsGlobalAdmin(string ID, string password)
        {
            return Users.Any(user => string.Equals(user.ID, ID) && string.Equals(user.Password, password) && (Role.GlobalAdmin== user.UserType));
        }
        public bool IsAdmin(string ID, string password, string bankID)
        {
            return Users.Any(user => string.Equals(user.ID, ID) && string.Equals(user.Password, password) && string.Equals(user.BankID, bankID) && (Role.admin == user.UserType));
        }

    }
}
