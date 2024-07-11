import datetime, os

class Product: # Create a Product class
    def __init__(self, discount_type, release_date, price, quantity_sold):
        self.discount_type = discount_type
        self.release_date = release_date
        self.price = price
        self.quantity_sold = quantity_sold

def calculate_total_sales(products): # Accept a list of Product objects
    total_sales = 0
    discount_percentage_B = 6
    discount_percentage_C = 2

    for product in products:
        if product.release_date.year >= 2020: # Filter by products released in 2020 or later
            if product.discount_type == 'B':
                discounted_price = product.price * (100 - discount_percentage_B / 100)
            elif product.discount_type == 'C':
                discounted_price = product.price * (100 - discount_percentage_C / 100)
            else:
                discounted_price = 0 # Ignore DiscountType "A"

            total_sales += discounted_price * product.quantity_sold

    return total_sales # Return the total sales for products featuring discount types B and C after discounts

def main():
    if os.path.exists("../ProdMasterlistB.txt"):
        with open("../ProdMasterlistB.txt", "r") as file:
            lines = file.readlines()
            products = []

            for line in lines[1:]: # Skip the header row (1st row)
                line = line.strip()
                product_info = line.split("|")
                product = Product(
                    product_info[7], # DiscountType
                    datetime.datetime.strptime(product_info[3], "%m/%d/%Y %I:%M:%S %p"), # ReleaseDate
                    float(product_info[5]), # Price
                    int(product_info[8]) # QuantitySold
                )
                products.append(product)

        total_sales = calculate_total_sales(products) # Get total sales based on the products list
        print(f"Total sales of products featuring discount types B and C after discounts: ${total_sales:.2f}")

    else:
        print("ProdMasterlistB.txt file not found.")

if __name__ == "__main__":
    main()
