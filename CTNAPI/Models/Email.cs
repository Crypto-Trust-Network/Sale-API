using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CTNAPI.Models
{
    /// <summary>
    /// The subscriber model
    /// </summary>
    public class Subscriber
    {
        /// <summary>
        /// Generated GUID on post
        /// </summary>
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Email of the sub
        /// </summary>
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}