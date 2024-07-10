import datetime, os

class Product:
    def __init__(self, discount_type, release_date, price, quantity_sold):
        self.discount_type = discount_type
        self.release_date = release_date
        self.price = price
        self.quantity_sold = quantity_sold

def calculate_total_sales(products):
    total_sales = 0
    current_year = datetime.datetime.now().year
    discount_types_of_interest = ['B', 'C']
    discount_percentage_B = 6
    discount_percentage_C = 2

    for product in products:
        if product.discount_type in discount_types_of_interest and product.release_date.year >= 2020:
            if product.discount_type == 'B':
                discounted_price = product.price * (1 - discount_percentage_B / 100)
            elif product.discount_type == 'C':
                discounted_price = product.price * (1 - discount_percentage_C / 100)
            else:
                discounted_price = product.price

            total_sales += discounted_price * product.quantity_sold

    return total_sales

def main():
    if os.path.exists("../ProdMasterlistB.txt"):
        with open("../ProdMasterlistB.txt", "r") as file:
            lines = file.readlines()
            products = []

            for line in lines[1:]:
                line = line.strip()
                product_info = line.split("|")
                product = Product(product_info[7], datetime.datetime.strptime(product_info[3], "%m/%d/%Y %I:%M:%S %p"), float(product_info[5]), int(product_info[8]))
                products.append(product)

        total_sales = calculate_total_sales(products)
        print(f"Total sales of products featuring discount types B and C after discounts: ${total_sales:.2f}")

    else:
        print("File not found.")

if __name__ == "__main__":
    main()
