using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using WebApiWalkthrough2.Models;

namespace WebApiWalkthrough2.Controllers
{
    [Authorize]
    public class CompanyController : ApiController
    {
        private WalkthroughDbContext _appDbContext;


        public WalkthroughDbContext ApplicationDbContext
        {
            get { return _appDbContext ?? Request.GetOwinContext().Get<WalkthroughDbContext>(); }
        }

        public IEnumerable<Company> Get()
        {
            var user = User;
            var currentUserId = User.Identity;
            return ApplicationDbContext.Companies;
        }

        public Company Get(int id)
        {
            var company = ApplicationDbContext.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return company;
        }

        public async Task<IHttpActionResult> Post(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument Null");
            }
            var companyExists = ApplicationDbContext.Companies.Any(c => c.Id == company.Id);
            if (companyExists)
            {
                return BadRequest("Exists");
            }

            ApplicationDbContext.Companies.Add(company);
            await ApplicationDbContext.SaveChangesAsync();

            return Ok();
        }

        public IHttpActionResult Put(Company company)
        {
            if (company == null)
            {
                return BadRequest("Argument Null");
            }
            var existing = ApplicationDbContext.Companies.FirstOrDefault(c => c.Id == company.Id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = company.Name;
            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var company = ApplicationDbContext.Companies.FirstOrDefault(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            ApplicationDbContext.Companies.Remove(company);
            return Ok();
        }
    }
}