using ChneseSaleApi.models;
using Microsoft.AspNetCore.Mvc;

namespace sale.buisness
{
    public interface IJwtTokenService
    {
        //string GenerateToken(string username);
        //string GenerateJwtToken(string username, List<string> roles);

        string GenerateToken(string username);
        string GenerateJwtToken(string username, string id, List<string> roles);

    }
}
