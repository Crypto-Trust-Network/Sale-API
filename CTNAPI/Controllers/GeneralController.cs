using CTNAPI.Models;
using CTNAPI.Models.View;
using CTNAPI.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;
using System.Numerics;

namespace CTNAPI.Controllers
{

    /// <summary>
    /// Controller contains the majority of endpoints for the frontend
    /// </summary>
    [RoutePrefix("api/ctn")]
    public class GeneralController : ApiController
    {

        /// <summary>
        /// One Ether used to convert WEI to ETH
        /// </summary>
        public const decimal ETHER = 1000000000000000000m;

        /// <summary>
        /// COntract Address
        /// </summary>
        public const string CONTRACT_SALE_ADDRESS = "0x491559dd3DfdBCA13EDc74569e86c8A0D517975b";


        /// <summary>
        /// The start of the sale
        /// </summary>
        DateTime start = new DateTime(2018, 01, 22, 18, 00, 00, DateTimeKind.Utc);


        /// <summary>
        /// Helper for making HTTP Requests
        /// </summary>
        RequestService service = new RequestService();

        /// <summary>
        /// Provides caching for objects
        /// </summary>
        private MemoryCache memCache = MemoryCache.Default;

        /// <summary>
        /// The rules around caching
        /// </summary>
        CacheItemPolicy cachePolicy = new CacheItemPolicy();

        /// <summary>
        /// Key for getting the object from cache
        /// </summary>
        public const string KEY = "TRANSACTIONS-";


        public GeneralController()
        {
            cachePolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
        }

        /// <summary>
        /// Get transactions to the contract address
        /// </summary>
        /// <param name="lastBlock">The last block that the requestor has transactions upto</param>
        /// <returns>Transactions after the last block</returns>
        [Route]
        [HttpGet]
        public IHttpActionResult Get(int lastBlock = 0)
        {
            CacheModel response = (CacheModel)memCache.Get(KEY);

            if (response == null)
            {
                response = GetFromEtherScan(CONTRACT_SALE_ADDRESS);
            }

            return Ok(response.transactions.Where(m => m.blockNumber > lastBlock).OrderBy(m => m.blockNumber).ToList());
        }


        /// <summary>
        /// Subscribers endpoint
        /// </summary>
        /// <param name="sub">The subscriber mode l</param>
        /// <returns>Status Ok</returns>
        [Route]
        [HttpPost]
        public IHttpActionResult Post(Subscriber sub)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                Subscriber existing = db.Subscribers.FirstOrDefault(m => m.Email == sub.Email);

                if (existing == null)
                {
                    db.Subscribers.Add(sub);
                    db.SaveChanges();
                }
            }

            return Ok();
        }

        /// <summary>
        /// Gets all transactions greater then the last block
        /// </summary> 
        /// <param name="lastBlock"></param>
        /// <returns></returns>
        private CacheModel GetFromEtherScan(string address)
        {

            CacheModel cache;

            //Dont get from ether unless the sale has begun
            if (DateTime.UtcNow >= start)
            {

                string json = service.Get("https://api.etherscan.io/api?module=account&action=txlist&address="+address+"&startblock=0&endblock=99999999&sort=asc&apikey=0xED3Eb6e24967915E27f790E5101815327a419528");

                Response response = JsonConvert.DeserializeObject<Response>(json);

                cache = new CacheModel(response, address);

            }
            else
            {
                cache = new CacheModel();
            }

            // Save the object to cache
            memCache.Add(KEY, cache, cachePolicy);

            return cache;
        }

    }
}
