import datetime, os
from functools import reduce

class Product: # Create a Product class
    def __init__(self, discount_type, release_date, price, quantity_sold):
        self.discount_type = discount_type
        self.release_date = release_date
        self.price = price
        self.quantity_sold = quantity_sold

def CalculateTotalSales(products): # Accept a list of Product objects
    return reduce(
        (lambda x, y : x + y), 
        map(lambda product : (product.price * (100 - 6 / 100) if product.discount_type == 'B' else (product.price * (100 - 2 / 100) if product.discount_type == 'C' else 0)) * product.quantity_sold, 
        filter(lambda product : product.release_date.year >= 2020, products))
    )

def main():
    if os.path.exists("../ProdMasterlistB.txt"):
        with open("../ProdMasterlistB.txt", "r") as file:
            lines = file.readlines()
            print(
                f"Total sales of products featuring discount types B and C after discounts: ${CalculateTotalSales(
                    list(map(lambda product : Product(
                        product.strip().split("|")[7], 
                        datetime.datetime.strptime(product.strip().split("|")[3], "%m/%d/%Y %I:%M:%S %p"), 
                        float(product.strip().split("|")[5]), 
                        int(product.strip().split("|")[8])
                    ), lines[1:]))
                ):.2f}"
            )
            file.close()
    else:
        print("ProdMasterlistB.txt file not found.")

if __name__ == "__main__":
    main()
