using Microsoft.Data.SqlClient;
using System.Data;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class UserLoginService : IUserLoginService
    {
        private readonly SqlConnection _sqlConn;
        private readonly ILogger<UserLoginService> _logger;
        public UserLoginService(SqlConnection sqlConn, ILogger<UserLoginService> logger)
        {
            _sqlConn = sqlConn;
            _logger = logger;
        }
        public async Task<UserLoginData?> GetUserLoginDataAsync(string username)
        {
            const string procedure = "User_GetLoginData";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 255) { Value = username });

                UserLoginData? userLoginData = null;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    userLoginData = new UserLoginData
                    {
                        HashedPassword = reader.GetString(reader.GetOrdinal("Password")),
                        Role = reader.GetString(reader.GetOrdinal("Role"))
                    };
                }
                
                return userLoginData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil data login untuk username: {u}", username);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}
