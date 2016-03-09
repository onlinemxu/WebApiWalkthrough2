using System.Web.Http;
using WebApiWalkthrough2.Infrastructure;

namespace WebApiWalkthrough2.Controllers
{
    [TestAuthorizationFilterAttribute]
    public class TestController : ApiController
    {
        public IHttpActionResult Get()
        {
            Helper.Write("Test Controller", User);

            return Ok();
        }
    }
}