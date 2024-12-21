using System;
using SV20T1020051.DataLayers;
using SV20T1020051.DataLayers.MySQL;
using SV20T1020051.DomainModels;

namespace SV20T1020051.BusinessLayers
{
	public class UserAccountService
	{
        private static readonly IUserAccountDAL employeeAccountDB;

        static UserAccountService()
        {
            employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAccountDB.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }
    }
}

