using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTNAPI.Models.View
{
    /// <summary>
    /// The model we receive to generate a bounty 
    /// </summary>
    public class BountyClaimPostModel
    {

        [Required]
        [EmailAddress(ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        
        [Required]
        public string Wallet { get; set; }

        [Required]
        public string Url { get; set; }
    }
}