using MonitorAPI.Dao.framework;
using MonitorAPI.Model;
using System.Data.SqlClient;

namespace MonitorAPI.Dao
{
    public class UserDao:BaseDao
    {
        private const string QUERY_BY_NameAndPwd_SQL = "SELECT * FROM [User] WHERE UserName=@Name";

        public UserDao(PersistenceContext pc):base(pc) { }
        public bool UserExists(string Name,string Password) {
            using (SqlCommand command = new SqlCommand()) {
                command.Connection = Connection;
                command.CommandText = QUERY_BY_NameAndPwd_SQL;
                command.Parameters.AddWithValue("@Name", Name);
                int result = (int)command.ExecuteScalar();
                return result ==1;
            }
        }

        public User GetUser(string Name)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_BY_NameAndPwd_SQL;
                command.Parameters.AddWithValue("@Name", Name);
                User user = SqlHelper.ExecuteReaderCmdObject<User>(command);
                return user;
            }
        }
    }
}
