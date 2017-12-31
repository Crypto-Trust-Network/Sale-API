using CTNAPI.Models;
using CTNAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web.Http;

namespace CTNAPI.Controllers
{
    [RoutePrefix("api/bounty")]
    public class BountiesController : ApiController
    {

        /// <summary>
        /// End point for recieving bounty requests.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route]
        [HttpPost]
        public IHttpActionResult Post(BountyClaimPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationDbContext db = new ApplicationDbContext();

            // Check to make sure the url does not already exist
            if (db.BountyClaims.Where(m => m.URL == model.Url).Count() > 0)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            // Add the new claim to the database
            db.BountyClaims.Add(new BountyClaim(model));

            db.SaveChanges();

            return Ok();
        }
    }
}
