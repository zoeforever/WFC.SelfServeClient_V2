using WebApiClient;
using WFC.ServerClient.Attributes;

namespace WFC.ServerClient
{
    [JwtFilter]
    public partial interface ISystemUserRoleApi : IHttpApi
    {
    }
}