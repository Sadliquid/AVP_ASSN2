﻿using SectionA;

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
        public static void Main(string[] args) {
            List<Product> products = SectionA.Program.ReadProdMasterList();
            if (products.Count == 0) {
                Console.WriteLine("No products to generate info for.");
                Console.WriteLine();
                return;
            } else {
                double totalDiscountedPrice = 0;
                for (int i = 0; i < products.Count; i++){
                    Product discountedProduct = CalculateDiscount(products[i], products[i].DiscountType);
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"{products[i].Name} ({products[i].Barcode})");
                    Console.WriteLine($"Price: ${products[i].Price}");
                    Console.WriteLine($"Discounted Price: ${discountedProduct.Price.ToString("F2")}");
                    Console.WriteLine("----------------------------------");
                    totalDiscountedPrice += discountedProduct.Price;
                }
                Console.WriteLine($"Total discount price: ${totalDiscountedPrice.ToString("F0")} for {products.Count} products.");
            }
        }
    }
}
