namespace Server_Side_Code_Aol_SoftEng.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(string username, string role);
        bool ValidateToken(string token);
        string GetUsernameFromToken(string token);
        string GetRoleFromToken(string token);
    }
}
