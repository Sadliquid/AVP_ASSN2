using SectionA;

namespace SectionB {
    public class Program {
        public enum DiscountTier { // Declare enum to store discount tiers
            A = 10,
            B = 6,
            C = 2
        }

        public static (Product, double) CalculateDiscount(Product product, string discountTier) {
            double originalPrice = product.Price;
            double discountedPercentage;

            if (discountTier == "A") {
                discountedPercentage = (100 - (int)DiscountTier.A) / 100.0; // Calculate percentage payable after discount
            } else if (discountTier == "B") {
                discountedPercentage = (100 - (int)DiscountTier.B) / 100.0; // Calculate percentage payable after discount
            } else {
                discountedPercentage = (100 - (int)DiscountTier.C) / 100.0; // Calculate percentage payable after discount
            }

            product.Price *= discountedPercentage;
            return (product, originalPrice); // Return both the discounted product and the original price
        }

        public static async Task<(Product, double)> CalculateDiscountAsync(Product product, string discountTier) { // Define a function to call the CalculateDiscount method asynchronously
            return await Task.Run(() => CalculateDiscount(product, discountTier));
        }

        public static void UpdateDiscAmountToMasterlist(string content) {
            File.AppendAllText(Path.GetFullPath("../ProdMasterlistB.txt"), content); // Create a new text file and write the updated content to it
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

                List<Task<(Product, double)>> discountTasks = new List<Task<(Product, double)>>(); // Initialise a list of tasks to complete
                foreach (var product in products) {
                    discountTasks.Add(CalculateDiscountAsync(product, product.DiscountType)); // Call the CalculateDiscount function asynchronously
                }

                (Product, double)[] discountedProducts = await Task.WhenAll(discountTasks); // Construct complete discountedproducts list only when async CalculateDiscount calls are completed, then proceed.

                for (int i = 0; i < products.Count; i++) { // discountedproducts list has already been fully constructed
                    var (discountedProduct, originalPrice) = discountedProducts[i];
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"{products[i].Name} ({products[i].Barcode})");
                    Console.WriteLine($"Price: ${originalPrice}");
                    Console.WriteLine($"Discounted Price: ${discountedProduct.Price.ToString("F2")}");
                    Console.WriteLine("----------------------------------");
                    totalDiscountedPrice += discountedProduct.Price;
                    UpdateDiscAmountToMasterlist($"{discountedProduct.Barcode}|{discountedProduct.Name}|{discountedProduct.Description}|{discountedProduct.ReleaseDate.ToString("MM/dd/yyyy")}|{discountedProduct.Feature}|{originalPrice.ToString("F2")}|{discountedProduct.Quantity}|{discountedProduct.DiscountType}|{discountedProduct.QuantitySold}|{discountedProduct.Weight}|{discountedProduct.PackagingMaterial}|{discountedProduct.Price.ToString("F2")}\n");
                }

                Console.WriteLine($"Total discount price: ${totalDiscountedPrice.ToString("F0")} for {products.Count} products.");
                Console.WriteLine();
                Console.WriteLine("Discounted products written to ProdMasterlistB.txt.");
                Console.WriteLine();
            }
        }
    }
}