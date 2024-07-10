using System;

namespace SectionA {
    public class Product {
        public required string Barcode { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required DateTime ReleaseDate { get; set; }
        public required string Feature { get; set; }
        public required double Price { get; set; }
        public required int Quantity { get; set; }
        public required string DiscountType { get; set; }
        public required int QuantitySold { get; set; }
        public required double Weight { get; set; }
        public required string PackagingMaterial { get; set; }
        public double DiscountPrice { get; set; } = 0.0;

        public string GenerateInfoForMarketing() {
            string generatedMarketingData =  $"{Name} - {Description} - {Price} - {Feature} - {ReleaseDate}";
            if (File.Exists("Marketing.txt")) {
                File.AppendAllText("Marketing.txt", generatedMarketingData + Environment.NewLine);
            } else {
                File.WriteAllText("Marketing.txt", generatedMarketingData + Environment.NewLine);
            }
            return generatedMarketingData;
        }

        public string GenerateInfoForSales() {
            string generatedSalesData = $"{Barcode} - {Name} - {Description} - {Price} - {Quantity} - {QuantitySold} - {DiscountType}";
            if (File.Exists("Sales.txt")) {
                File.AppendAllText("Sales.txt", generatedSalesData + Environment.NewLine);
            } else {
                File.WriteAllText("Sales.txt", generatedSalesData + Environment.NewLine);
            }
            return generatedSalesData;
        }

        public string GenerateInfoForLogistics() {
            string generatedLogisticsData = $"{Barcode} - {Name} - {Weight} - {PackagingMaterial}";
            if (File.Exists("Logistics.txt")) {
                File.AppendAllText("Logistics.txt", generatedLogisticsData + Environment.NewLine);
            } else {
                File.WriteAllText("Logistics.txt", generatedLogisticsData + Environment.NewLine);
            }
            return generatedLogisticsData;
        }
    }
}