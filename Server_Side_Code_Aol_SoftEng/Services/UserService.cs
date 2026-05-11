using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;

namespace Server_Side_Code_Aol_SoftEng.Services
{
    public class UserService : IUserService
    {
        private readonly SqlConnection _sqlConn;
        private readonly ILogger<UserService> _logger;
        public UserService(SqlConnection sqlConn, ILogger<UserService> logger)
        {
            _sqlConn = sqlConn;
            _logger = logger;
        }

        public async Task CreateUserAsync(UserCreateModel data, string hashedPassword)
        {
            const string procedure = "User_CreateUser";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 255) { Value = data.Username });
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = hashedPassword });
                command.Parameters.Add(new SqlParameter("@Role", SqlDbType.VarChar, 50) { Value = data.Role });

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Menambah user baru dengan username: {u} dan role: {r}", 
                    data.Username, data.Role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat membuat user baru dengan username: {u} dan role: {r}", 
                    data.Username, data.Role);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}
