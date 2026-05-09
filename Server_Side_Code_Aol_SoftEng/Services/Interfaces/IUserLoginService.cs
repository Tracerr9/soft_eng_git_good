using Server_Side_Code_Aol_SoftEng.Models;

namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface IUserLoginService
    {
        Task<UserLoginData> GetUserLoginDataAsync(string username);
    }
}
