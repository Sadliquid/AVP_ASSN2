using SectionA;
using System.Threading.Tasks;

namespace SectionB {
    public class Program {
        public enum DiscountTier {
            A = 10,
            B = 6,
            C = 2
        }

        public static Product CalculateDiscount(Product product, string discountTier) {
            double discountedPercentage;
            if (discountTier == "A") {
                discountedPercentage = (100 - (int)DiscountTier.A) / 100.0;
            } else if (discountTier == "B") {
                discountedPercentage = (100 - (int)DiscountTier.B) / 100.0;
            } else {
                discountedPercentage = (100 - (int)DiscountTier.C) / 100.0;
            }
            product.Price *= discountedPercentage;
            return product;
        }

        public static async Task<Product> CalculateDiscountAsync(Product product, string discountTier) {
            return await Task.Run(() => CalculateDiscount(product, discountTier));
        }

        public static void UpdateDiscAmountToMasterlist(string content) {
            File.AppendAllText(Path.GetFullPath("../ProdMasterlistB.txt"), content);
        }

        public static async Task Main(string[] args) {
            List<Product> products = SectionA.Program.ReadProdMasterList();
            if (products.Count == 0) {
                Console.WriteLine("No products to generate info for.");
                Console.WriteLine();
                return;
            } else {
                double totalDiscountedPrice = 0;
                File.WriteAllText(Path.GetFullPath("../ProdMasterlistB.txt"), "Barcode|Name|Description|ReleaseDate(dd/MM/yyyy)|Feature|Price(S$)|Quantity|DiscountType|QuantitySold|Weight(kg)|PackagingMaterial|DiscountedPrice(S$)\n");

                List<Task<Product>> discountTasks = new List<Task<Product>>();
                foreach (var product in products) {
                    discountTasks.Add(CalculateDiscountAsync(product, product.DiscountType));
                }

                Product[] discountedProducts = await Task.WhenAll(discountTasks);

                for (int i = 0; i < products.Count; i++){
                    Product discountedProduct = discountedProducts[i];
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"{products[i].Name} ({products[i].Barcode})");
                    Console.WriteLine($"Price: ${products[i].Price}");
                    Console.WriteLine($"Discounted Price: ${discountedProduct.Price.ToString("F2")}");
                    Console.WriteLine("----------------------------------");
                    totalDiscountedPrice += discountedProduct.Price;
                    UpdateDiscAmountToMasterlist($"{discountedProduct.Barcode}|{discountedProduct.Name}|{discountedProduct.Description}|{discountedProduct.ReleaseDate}|{discountedProduct.Feature}|{products[i].Price.ToString("F2")}|{discountedProduct.Quantity}|{discountedProduct.DiscountType}|{discountedProduct.QuantitySold}|{discountedProduct.Weight}|{discountedProduct.PackagingMaterial}|{discountedProduct.Price.ToString("F2")}\n");
                }

                Console.WriteLine($"Total discount price: ${totalDiscountedPrice.ToString("F0")} for {products.Count} products.");
                Console.WriteLine();
                Console.WriteLine("Discounted products written to ProdMasterlistB.txt.");
                Console.WriteLine();
            }
        }
    }
}
