namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface IUserLoginService
    {
        Task<string> GetUserHashedPasswordAsync(string username);
    }
}
