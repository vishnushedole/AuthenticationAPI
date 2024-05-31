using AuthenticationAPI.Data;
using AuthenticationAPI.Model;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using WebClassLibrary;

namespace AuthenticationAPI.Services
{
    public interface IAuthServiceAsync
    {
        Task<UserOrManager> AuthenticateAsync(AuthRequest model);
        Task<UserOrManager> GetUserDetails(int userId);
        Task<ActionResult<int>> GetCustomerId(int userId);
        Task<ActionResult<EmployeeRes>> GetEmployeeId(string userName);
    }

 

    public class UserService : IAuthServiceAsync
    {
        private readonly List<UserOrManager> users;
        /*public UserService()
        {
            // Initialize the users list in the constructor
            users = new List<UserOrManager>
            {
                new UserOrManager { userid = 1, username = "Customer123", password = "C12345", roleid= 0 },
                new UserOrManager { userid = 2, username = "manager@gmail.com", password = "RM12345",roleid= 1 }
            };
        }*/
        private readonly BaseDataAccess access;
        public UserService()
        {
            access = new BaseDataAccess();
        }
        
        public async Task<UserOrManager> AuthenticateAsync(AuthRequest model)
        {
            /*var user = users.FirstOrDefault(c => c.username == model.username && c.password == model.password);
            return Task.Run(() => user);*/

            // Connecting To ADO.NET
            UserOrManager user = null!;
            var sqlText = "sp_AuthenticateUser";

            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter[]
                    {
                new SqlParameter("@Username", model.username),
                new SqlParameter("@Password", model.password)
                    }!);

                while (await reader.ReadAsync())
                {
                    user = new UserOrManager
                    {
                        userid = (int)reader["userid"],
                        username = (string)reader["username"],
                        password = (string)reader["password"],
                        roleid = (int)reader["roleid"]
                       
                    };
                    
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return user;
        }
        
        public async Task<UserOrManager> GetUserDetails(int userId)
        {
            /*var user = users.FirstOrDefault(c => c.userid == userId);
            return Task.Run(() => user);*/

            // ADO.NET connection

            UserOrManager user = null!;
            var sqlText = "sp_GetUserDetails";

            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter[]
                    {
                new SqlParameter("@UserId", userId)
                    }!);

                while (await reader.ReadAsync())
                {
                    user = new UserOrManager
                    {
                        userid = (int)reader["userid"],
                        username = (string)reader["username"],
                        password = (string)reader["password"],
                        roleid = (int)reader["roleid"]
                    };
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return user;
        }
        public async Task<ActionResult<int>> GetCustomerId(int userId)
        {
            

          
            var sqlText = "sp_GetCustomerId";
            var CustomerId = 0;
            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter[]
                    {
                new SqlParameter("@UserId", userId)
                    }!);

                while (await reader.ReadAsync())
                {
                    CustomerId = reader.GetInt32(0);
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return CustomerId;
        }
        public async Task<ActionResult<EmployeeRes>> GetEmployeeId(string userName)
        {



            var sqlText = "sp_GetEmployeeId";
            var Emp = new EmployeeRes();
            try
            {
                var reader = access.ExecuteReader(
                    sqltext: sqlText,
                    commandType: CommandType.StoredProcedure,
                    parameters: new SqlParameter[]
                    {
                new SqlParameter("@UserName", userName)
                    }!);

                while (await reader.ReadAsync())
                {
                    Emp.Id = reader.GetInt32(0);
                    Emp.Name = reader.GetString(1);
                }

                if (!reader.IsClosed)
                    reader.Close();
            }
            catch (SqlException sqle)
            {
                // Log or handle the exception
                throw;
            }
            catch (Exception e)
            {
                // Log or handle the exception
                throw;
            }
            finally
            {
                access.CloseConnection();
            }

            return Emp;
        }

    }
}
