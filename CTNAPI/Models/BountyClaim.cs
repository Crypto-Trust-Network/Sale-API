using CTNAPI.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTNAPI.Models
{
    /// <summary>
    /// Bounty Model
    /// </summary>
    public class BountyClaim
    {
        /// <summary>
        /// The Id for the claim
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The email of the user making the claim
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The address of the wallet to deposit the tokens to
        /// </summary>
        public string WalletAddress { get; set; }

        /// <summary>
        /// The URL of the post to check
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// The time the claim was made
        /// </summary>
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Required public empty constructor
        /// </summary>
        public BountyClaim() { }

        /// <summary>
        /// Constructor based on post model
        /// </summary>
        /// <param name="post">The model post from the front end</param>
        public BountyClaim(BountyClaimPostModel post)
        {
            Email = post.Email;
            WalletAddress = post.Wallet;
            URL = post.Url;
        }
    }
}