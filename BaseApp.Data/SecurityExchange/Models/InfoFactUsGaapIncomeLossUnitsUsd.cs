using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Data.SecurityExchange.Models
{
    public class InfoFactUsGaapIncomeLossUnitsUsd
    {
        public int Id { get; set; }

        public int InfoFactUsGaapIncomeLossUnitsId { get; set; }

        /// <summary>
        /// Possibilities include 10-Q, 10-K,8-K, 20-F, 40-F, 6-K, and
        /// their variants.YOU ARE INTERESTED ONLY IN 10-K DATA!
        /// </summary>
        public string Form { get; set; }
        
        /// <summary>
        /// For yearly information, the format is CY followed by the year
        /// number.For example: CY2021.YOU ARE INTERESTED ONLY IN YEARLY INFORMATION
        /// WHICH FOLLOWS THIS FORMAT!
        /// </summary>
        public string Frame { get; set; }

        /// <summary>
        /// The income/loss amount.
        /// </summary>
        public decimal Val { get; set; }
    }
}
