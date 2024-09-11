/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ServicesDomain.Enums;

namespace ServicesDomain.Database.Data
{
    /// <summary>
    /// This class contains standart marketplace cards presentation
    /// </summary>
    public class ProductCard
    {
        /// <summary>
        /// Return true if card doesn't have _id from data base
        /// </summary>
        public bool IsEmpty()
        {
            return _id == "";
        }

        /// <summary>
        /// Data base index
        /// </summary>
        public string _id { get; set; } = "";
        /// <summary>
        /// Data base index of parent onion
        /// </summary>
        public string parent_onion_id { get; set; } = "";
        /// <summary>
        /// Seller articul
        /// </summary>
        public string articul { get; set; } = "";
        /// <summary>
        /// Inner marketplace articul
        /// </summary>
        public string marketplace_articul { get; set; } = "";
        /// <summary>
        /// Product card name determines by seller in marketplace
        /// </summary>
        public string name { get; set; } = "";
        /// <summary>
        /// Current product card stocks
        /// </summary>
        public int stocks { get; set; } = 0;
        /// <summary>
        /// Product card creation data from marketplace
        /// </summary>
        public DateTime creation_date { get; set; }
        /// <summary>
        /// Photo link from marketplace
        /// </summary>
        public string photo { get; set; } = "";
        /// <summary>
        /// Flag for change stocks in other cards in onion. If false update only this card
        /// </summary>
        public bool is_synch { get; set; } = true;
        /// <summary>
        /// Count of products for update other cards in onion.
        /// Example: 2 cards of 1 product, different by count.
        /// This count should be indicated in the multiplicity
        /// </summary>
        public int multiplicity { get; set; } = 1;
        /// <summary>
        /// Marketplace where product card from
        /// </summary>
        public EMarketplaces marketplace { get; set; }
        /// <summary>
        /// Barcodes of product card
        /// </summary>
        public List<string> barcodes { get; set; } = new();
    }
}
