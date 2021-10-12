using Pospos.Domain.Entities;

namespace Pospos.Api.Models.Response
{
    public class LoginRespModel : BaseApiModel
    {
        public Users User { get; set; }
    }
}
