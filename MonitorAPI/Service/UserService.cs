using MonitorAPI.Dao;
using MonitorAPI.Dao.framework;
using MonitorAPI.Model;
using System;

namespace MonitorAPI.Service
{
    public class UserService
    {
        public User UserLogin(string userName)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                UserDao userDao = new UserDao(pc);
                User user = userDao.GetUser(userName);
                return user;
            }
        }
    }
}
