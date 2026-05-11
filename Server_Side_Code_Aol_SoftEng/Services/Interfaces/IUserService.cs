using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserCreateModel data, string hashedPassword);
    }
}
