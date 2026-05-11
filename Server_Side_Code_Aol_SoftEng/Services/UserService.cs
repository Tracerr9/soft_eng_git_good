using Microsoft.Data.SqlClient;
using Server_Side_Code_Aol_SoftEng.Models;
using Server_Side_Code_Aol_SoftEng.Services.Interfaces;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task UpdateUserAsync(UserEditModel data, string? hashedPassword, int userId)
        {
            const string procedure = "User_UpdateUser";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 255) { Value = data.Username });
                command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = hashedPassword is null ? 
                    DBNull.Value : hashedPassword });
                command.Parameters.Add(new SqlParameter("@Role", SqlDbType.VarChar, 50) { Value = data.Role });

                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Update user dengan id: {i} menjadi username: {u} dan role: {r}",
                    userId, data.Username, data.Role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat update user dengan id: {i} menjadi username: {u} dan role: {r}",
                    userId, data.Username, data.Role);
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            const string procedure = "User_GetAll";
            try
            {
                await _sqlConn.OpenAsync();
                using var command = new SqlCommand(procedure, _sqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using var reader = await command.ExecuteReaderAsync();

                var users = new List<UserDto>();

                while (await reader.ReadAsync())
                {
                    users.Add(new UserDto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Role = reader.GetString(reader.GetOrdinal("Role"))
                    });
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengambil users.");
                throw;
            }
            finally
            {
                await _sqlConn.CloseAsync();
            }
        }
    }
}
