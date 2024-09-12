/**
 * @QumartSeller_Client
 * https://github.com/Nailed34/QumartSeller_Server-demo.git
 *
 * Copyright (c) 2024 https://github.com/Nailed34
 * Released under the MIT license
 */

using ClientServerConnection;
using ClientServerConnection.Actions;

namespace DemoServiceNamespace
{
    public class DemoService
    {
        public int GetProductsCount()
        {
            return Data.Count;
        }

        public List<ProductInfo> GetProductsInfo(int firstIndex, int lastIndex)
        {
            if (Data.Count < firstIndex)
                return new();

            var resultCount = lastIndex - firstIndex + 1;
            return Data.Slice(firstIndex - 1, Math.Min(resultCount, Data.Count - firstIndex + 1));
        }

        public bool AuthorizeUser(InUserAuthorization clientUser)
        {
            return clientUser.Username == "testlogin" && clientUser.Password == "testpassword";
        }

        private List<ProductInfo> Data = new();

        private string[] PhotoData = 
        {
            "https://images.unsplash.com/photo-1523275335684-37898b6baf30",
            "https://images.unsplash.com/photo-1504274066651-8d31a536b11a",
            "https://images.unsplash.com/photo-1549049950-48d5887197a0",
            "https://images.unsplash.com/photo-1509695507497-903c140c43b0",
            "https://images.unsplash.com/photo-1491637639811-60e2756cc1c7",
            "https://images.unsplash.com/photo-1486401899868-0e435ed85128",
            "https://images.unsplash.com/photo-1608248543803-ba4f8c70ae0b",
            "https://images.unsplash.com/photo-1580870069867-74c57ee1bb07",
            "https://images.unsplash.com/photo-1564278047230-a632a9d6acf4",
            "https://images.unsplash.com/photo-1560343090-f0409e92791a",
            "https://images.unsplash.com/photo-1491553895911-0055eca6402d",
            "https://images.unsplash.com/photo-1492707892479-7bc8d5a4ee93",
            "https://images.unsplash.com/photo-1608571423902-eed4a5ad8108",
            "https://images.unsplash.com/photo-1597931752949-98c74b5b159f",
            "https://images.unsplash.com/photo-1523027737707-96c0e1fd54e4",
            "https://images.unsplash.com/photo-1529634885322-b17ffaf423ac",
            "https://images.unsplash.com/photo-1504707748692-419802cf939d"
        }; 

        private string GenerateBarcode(Random random)
        {
            string result = "420";
            for (int i = 0; i < 10; i++)
                result += random.Next(0, 10);
            return result;
        }
        private string GenerateOzonBarcode(Random random)
        {
            string result = "OZN";
            for (int i = 0; i < 9; i++)
                result += random.Next(0, 10);
            return result;
        }

        public DemoService()
        {
            Random random = new Random();

            for (int i = 0; i < 120; i++)
            {
                int cardsNumber = random.Next(1, 4);
                string articul = random.Next(10000, 100000).ToString();
                string mainBarcode = GenerateBarcode(random);
                int stocks = random.Next(0, 10);

                var newProduct = new ProductInfo();
                newProduct.articuls.Add(articul);
                newProduct.id = (i + 1).ToString();
                newProduct.barcodes.Add(mainBarcode);
                newProduct.name = "Product card #" + (i + 1).ToString();
                newProduct.stocks = stocks;
                // Random photo
                //newProduct.photo = PhotoData[random.Next(0, PhotoData.Length)];

                // Sequence photo
                newProduct.photo = PhotoData[i % PhotoData.Length];

                for (int j = 0; j < cardsNumber; j++)
                {
                    var newInfoCard = new ProductInfoCard();
                    newInfoCard.articul = articul;
                    newInfoCard.multiplicity = 1;
                    newInfoCard.creation_date = DateTime.Now;
                    newInfoCard.stocks = stocks;
                    newInfoCard.is_synch = true;
                    newInfoCard.marketplace = (EMarketplaces)j + 1;
                    newProduct.marketplaces.Add(newInfoCard.marketplace);
                    newInfoCard.name = newInfoCard.marketplace.ToString() + " Test card";
                    newInfoCard.barcodes.Add(mainBarcode);
                    if (newInfoCard.marketplace == EMarketplaces.Ozon)
                    {
                        int randomEanType = random.Next(1, 10);
                        if (randomEanType == 1)
                        {
                            var oznBarcode = GenerateOzonBarcode(random);
                            newInfoCard.barcodes.Add(oznBarcode);
                            newProduct.barcodes.Add(oznBarcode);
                        }
                    }

                    newProduct.cards.Add(newInfoCard);
                }
                Data.Add(newProduct);
            }
        }
    }
}
