namespace SectionA {
    public delegate string ProductInfoGenerator(Product product); // Declare the Delegate
    public class Program {
        static List<Product> products = new List<Product>();

        public static List<Product> ReadProdMasterList() {
            string filePath = Path.GetFullPath("../ProdMasterlist.txt");

            if (File.Exists(filePath)) {
                string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
                try {
                    foreach (string line in lines) {
                        string[] data = line.Split('|');
                        Product product = new Product() {
                            Barcode = data[0],
                            Name = data[1],
                            Description = data[2],
                            ReleaseDate = DateTime.ParseExact(data[3], "dd/MM/yyyy", null),
                            Feature = data[4],
                            Price = double.Parse(data[5]),
                            Quantity = int.Parse(data[6]),
                            DiscountType = data[7],
                            QuantitySold = int.Parse(data[8]),
                            Weight = double.Parse(data[9]),
                            PackagingMaterial = data[10]
                        };
                        products.Add(product);
                    }
                    Console.WriteLine($"{products.Count} products added to list.");
                    Console.WriteLine();
                    return products;
                } catch {
                    Console.WriteLine("Error retrieving all products from ProdMasterlist.txt.");
                    Console.WriteLine();
                    return new List<Product>();
                }
            } else {
                Console.WriteLine($"ProdMasterlist.txt does not exist.");
                Console.WriteLine();
                return new List<Product>();
            }
        }
        public static void Main(string[] args) {
            List<Product> products = ReadProdMasterList();
            if (products.Count == 0) {
                Console.WriteLine("No products to generate product info for.");
                Console.WriteLine();
                return;
            } else {
                try {
                    File.WriteAllText("Marketing.txt", "Name|Description|Price|Feature|ReleaseDate\n");
                    File.WriteAllText("Sales.txt", "Barcode|Name|Description|Price|Quantity|QuantitySold|DiscountType\n");
                    File.WriteAllText("Logistics.txt", "Barcode|Name|Weight|PackagingMaterial\n");
                    Console.WriteLine("All Files cleared.");
                    Console.WriteLine();
                } catch {
                    Console.WriteLine("Error clearing files.");
                    Console.WriteLine();
                }

                ProductInfoGenerator consolidatedDelegate = product => product.GenerateInfoForMarketing(); // Assign the first method to the ProductInfoGenerator delegate
                consolidatedDelegate += product => product.GenerateInfoForSales(); // Add the second method to the ProductInfoGenerator delegate
                consolidatedDelegate += product => product.GenerateInfoForLogistics(); // Add the third method to the ProductInfoGenerator delegate

                try {
                    foreach (Product product in products) {
                        consolidatedDelegate(product); // Invoke all 3 GenerateInfo methods together in 1 Delegate
                    }
                    Console.WriteLine("Product info generated and appended to text files.");
                    Console.WriteLine();
                } catch {
                    Console.WriteLine("Error generating product info and appending to their respective text files.");
                    Console.WriteLine();
                }
            }
        }
    }
}
