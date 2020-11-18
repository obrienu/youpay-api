using Youpay.API.Dtos;

namespace Youpay.API.Utils
{
    public interface ITokenUtil
    {
         string GenerateToken(TokenClaimsDto tokenClaimsDto);
    }
}