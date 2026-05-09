using Microsoft.Data.SqlClient;
using System.Data;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;

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
        public async Task<string> GetUserHashedPassword(string username)
        {
            const string procedure = "User_GetPassword";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn);
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 255) { Value = username });

                string hashedPassword = Convert.ToString(await command.ExecuteScalarAsync());
                
                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil password untuk username: {u}", username);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}
