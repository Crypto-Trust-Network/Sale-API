using CTNAPI.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTNAPI.Models.View
{

    /// <summary>
    /// The response from EtherScan
    /// </summary>
    public class Response
    {
        public string status { get; set; }

        public string message { get; set; }

        public List<EtherScanTransaction> result { get; set; }
    }

    public class EtherScanTransaction
    {
        public enum StatusEnum
        {
            Ok,
            Error
        }


        public int blockNumber { get; set; }

        public string hash { get; set; }

        public StatusEnum isError { get; set; }

        public int txreceipt_status { get; set; }

        public int timestamp { get; set; }

        public string to { get; set; }

        public string from { get; set; }

        public string value { get; set; }
    }

    /// <summary>
    /// The model we cache 
    /// </summary>
    public class CacheModel
    {
        public string status { get; set; }

        public List<ResponseTransaction> transactions { get; set; } = new List<ResponseTransaction>();

        public CacheModel()
        {
            status = "OK";
        }

        /// <summary>
        /// Constructor taking response from Etherscan. Filters results to incoming transactions that are only ETH contributions.
        /// </summary>
        /// <param name="response">Etherscan Repsonse</param>
        /// <param name="address">Contract Address</param>
        public CacheModel(Response response, string address)
        {

            status = response.status;

            transactions = response.result.Where(m => m.from != address && !string.IsNullOrEmpty(m.to) && m.value != "0").Select(m => new ResponseTransaction
            {
                blockNumber = m.blockNumber,
                from = m.from,
                hash = m.hash,
                isError = m.isError,
                txreceipt_status = m.txreceipt_status,
                timestamp = m.timestamp,
                to = m.to,
                value = decimal.Parse(m.value) / GeneralController.ETHER
            }).ToList();
        }
    }

    /// <summary>
    /// The transaction format which we return
    /// </summary>
    public class ResponseTransaction
    {

        public int blockNumber { get; set; }

        public string hash { get; set; }

        public EtherScanTransaction.StatusEnum isError { get; set; }

        public int txreceipt_status { get; set; }

        public int timestamp { get; set; }

        public string to { get; set; }

        public string from { get; set; }

        public decimal value { get; set; }
    }
}
