using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTemplateLibrary.Models;
using DataAccessLibrary.DAOS;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Security;
using System.Linq.Expressions;

namespace DataAccessLibrary.DBChildManagers
{
    /// <summary>
    /// This is a cascade that contains methods which are needed for DBManager.
    /// Works with DBUser datatype
    /// </summary>
    /// <Creator>Anton Kalashnikov</Creator>
    public class DBUserManager
    {
        UsersDAO usersDAO = new UsersDAO();

        public ReturnData<DBUser?, string> SingUpUser(DBUser user)
        {
            if (usersDAO.GetByName(user) != null) throw new Exception("User already exists in the database");
            int id = usersDAO.Create(user);
            user.ID = id;
            return new ReturnData<DBUser?, string>(user, "Signed up");
        }

        public DBUser? GetUserByName(string name)
        {
            DBUser returned = usersDAO.GetByName(name);
            try
            {
                if (returned.ID < 0);
            } catch (NullReferenceException e)
            {
                throw new Exception("User wasnt found in the database");
            }
            return returned;
        }

        public void RemoveUser(int userId)
        {
            usersDAO.Delete(userId);
        }

        public DBUser? LogUserIn(DBUser hypothetical_user)
        {
            DBUser user_in_db = GetUserByName(hypothetical_user.Name);
            // CHECK : Might get error instead of null from db if user is null
            if (user_in_db == null) throw new NullReferenceException("User doesnt exist in the database");
            if (!user_in_db.HashedPassword.ToLower().Equals(hypothetical_user.HashedPassword.ToLower())) throw new PasswordException("Users password is not right");
            return user_in_db;
        }

        public DBUser? GetUserById(int userId)
        {
            try
            {
                return usersDAO.GetByID(userId);
            } catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public bool UserExists(int userId)
        {
            return GetUserById(userId) != null;
        }

        public void UpdateUser(int userId, DBUser newUserData)
        {
            newUserData.ID = userId;
            usersDAO.Save(newUserData);
        }
    }
}
