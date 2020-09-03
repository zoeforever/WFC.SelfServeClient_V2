using System.Threading.Tasks;
using WebApiClient.Attributes;
using WebApiClient.Contexts;

namespace WFC.ServerClient.Attributes
{
    public class JwtFilterAttribute : ApiActionFilterAttribute
    {
        public override Task OnBeginRequestAsync(ApiActionContext context)
        {
            context.RequestMessage.Headers.Add("authorization", "Bearer " + WebApiClientHelper.Jwt);
            return Task.CompletedTask;
        }
    }
}
