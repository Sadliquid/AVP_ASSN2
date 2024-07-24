import datetime, os
from functools import reduce

class Product: # Create a Product class
    def __init__(self, discount_type, release_date, discountedPrice, quantity_sold):
        self.discount_type = discount_type
        self.release_date = release_date
        self.discountedPrice = discountedPrice
        self.quantity_sold = quantity_sold

def CalculateTotalSales(products):
    return reduce(
        lambda accumulator, product: accumulator + (product.discountedPrice * (product.quantity_sold * 1.35)),
        filter(
            lambda product: product.discount_type in ["B", "C"] and datetime.datetime.strptime(product.release_date, "%m/%d/%Y").year >= 2020,
            products
        ), 0
    )    

def main():
    if os.path.exists("../ProdMasterlistB.txt"):
        with open("../ProdMasterlistB.txt", "r") as file:
            lines = file.readlines()
            print(
                f"Total sales of products featuring discount types B and C after discounts: ${CalculateTotalSales(
                    list(map(lambda product : Product(
                        product.strip().split("|")[7], # Discount Type
                        product.strip().split("|")[3], # Release Date
                        float(product.strip().split("|")[11]), # Discounted Price
                        int(product.strip().split("|")[8]) # Quantity Sold
                    ), lines[1:]))
                ):.2f}"
            ) # Print total sales based on a list of products using Lambda map in 1 statement
            file.close()
    else:
        print("ProdMasterlistB.txt file not found.")

if __name__ == "__main__":
    main()