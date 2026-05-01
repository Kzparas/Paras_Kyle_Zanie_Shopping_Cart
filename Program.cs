using System;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int Stock;
    public string Category;

    public Product (int id, string name, string category, double price, int stock)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        Stock = stock;
    }

    public void DisplayProduct()
    {
        if (Stock > 0)
        {
            Console.WriteLine(Id + ". " + Name + " (" + Category + ") - PHP" + Price + " (" + Stock + " in stock)");
        }
        else
        {
            Console.WriteLine(Id + ". " + Name + " (" + Category + ") - PHP" + Price + " (Out of stock)");
        }
    }
    public double GetTotal(int qty)
    { 
        return Price * qty;
    }

    public bool HasenoughStock(int qty)
    {
        return qty <= Stock;
    }
    public void deductStock(int qty)
    {
        Stock -= qty;
    }
}

internal class NewBaseType
{
    static void Main()
    {
        Product[] products = [
            new Product(1, "Tshirt", "topwear", 500, 15),
            new Product(2, "Sweater", "topwear", 600, 17),
            new Product(3, "Hoodie", "topwear", 750, 12),
            new Product(4, "Jeans", "bottomwear", 1200, 16),
            new Product(5, "Shoes", "footwear", 1500, 22),
            new Product(6, "Closed cap", "accessories", 300, 10),
            new Product(7, "Jorts", "bottomwear", 200, 23)
        ];

        int[] cartQty = new int[products.Length];
        double[] orderHistory = new double[10];
        int orderCount = 0;
        int receiptNo = 1;

        string choice = "Y";

        while (choice == "Y")
        {
            Console.WriteLine("\n====== MAIN MENU ======");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Checkout");
            Console.WriteLine("5. View Order History");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine();

            // ============== VIEW PRODUCTS ==============
            if (option == "1")
            {
                Console.WriteLine("--- PRODUCTS LISTS ---");
                for (int i = 0; i < products.Length; i++)
                    products[i].DisplayProduct();

                Console.Write("Enter product ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id)) continue;

                if (id < 1 || id > products.Length) continue;

                Product p = products[id - 1];

                if (p.Stock == 0)
                {
                    Console.WriteLine("Out of stock.");
                    continue;
                }
                Console.Write("Enter quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int qty)) continue;

                if (qty <= 0) continue;

                if (p.HasenoughStock(qty))
                {
                    cartQty[id - 1] += qty;
                    p.deductStock(qty);
                    Console.WriteLine("Added to cart.");
                }
                else
                {
                    Console.WriteLine("Not enough stock.");
                }
            }
            // ============== SEARCH PRODUCT ============== 
            else if (option == "2")
            {
                Console.Write("Enter product name: ");
                string search = Console.ReadLine().ToLower();

                bool found = false;
                Console.WriteLine("--- SEARCH RESULTS ---");
                for (int i = 0; i < products.Length; i++)
                {
                    if (products[i].Name.ToLower().Contains(search))
                    {
                        products[i].DisplayProduct();
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("Product not found.");
            }
            // ============== VIEW CART ==============
            else if (option == "3")
            {
                Console.WriteLine("--- YOUR CART ---");

                for (int i = 0; i < products.Length; i++)
                {
                    if (cartQty[i] > 0)
                    {
                        Console.WriteLine(products[i].Name + " x " + cartQty[i]);

                    }
                }

                Console.WriteLine("\n1. Remove item");
                Console.WriteLine("2. Update quantity");
                Console.WriteLine("3. Clear cart");
                Console.WriteLine("4. Back to main menu");
                Console.Write("Choose: ");
                string cartOption = Console.ReadLine();

                if (cartOption == "1")
                {
                    Console.Write("Enter product ID to remove: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) continue;
                    if (id < 1 || id > products.Length || cartQty[id - 1] == 0) continue;

                    products[id - 1].Stock += cartQty[id - 1];
                    cartQty[id - 1] = 0;

                    Console.WriteLine("Item removed from cart.");
                }
                else if (cartOption == "2")
                {
                    Console.Write("Enter product ID to update: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) continue;

                    Console.Write("Enter new quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int newQty)) continue;

                    if (newQty < 0) continue;

                    int oldQty = cartQty[id - 1];
                    products[id - 1].Stock += oldQty;

                    if (products[id - 1].HasenoughStock(newQty))
                    {
                        cartQty[id - 1] = newQty;
                        products[id - 1].deductStock(newQty);

                        Console.WriteLine("Cart updated.");
                    }
                    else
                    {
                        products[id - 1].deductStock(oldQty);
                        Console.WriteLine("Not enough stock.");
                    }
                }
                else if (cartOption == "3")
                {
                    for (int i = 0; i < cartQty.Length; i++)
                    {
                        products[i].Stock += cartQty[i];
                        cartQty[i] = 0;
                    }
                    Console.WriteLine("Cart cleared.");
                }
            }
            // ============== CHECKOUT ==============
            else if (option == "4")
            {
                bool hasItem = false;
                for (int i = 0; i < cartQty.Length; i++)                {
                    if (cartQty[i] > 0)
                    {
                        hasItem = true;
                        break;
                    }
                }
                double grandtotal = 0;
                Console.WriteLine("\n--- RECEIPT ---");
                Console.WriteLine("Receipt No: " + receiptNo);
                Console.WriteLine("Date: " + DateTime.Now);
                Console.WriteLine("-----------------------------");

                for (int i = 0; i < products.Length; i++)
                {
                    if (cartQty[i] > 0)
                    {
                        double total = products[i].GetTotal(cartQty[i]);
                        Console.WriteLine(products[i].Name + " x " + cartQty[i] + " = PHP" + total);
                        grandtotal += total;
                    }
                }

                double discount = (grandtotal >= 5000) ? grandtotal * 0.10 : 0;
                double netTotal = grandtotal - discount;

                Console.WriteLine("-----------------------------");
                Console.WriteLine("\nTotal: PHP" + grandtotal);
                Console.WriteLine("Discount: PHP" + discount);
                Console.WriteLine("Final: PHP " + netTotal);

                double payment;
                while (true)                {
                    Console.Write("Enter payment amount: ");
                    if (!double.TryParse(Console.ReadLine(), out payment)) continue;

                    if (payment >= netTotal) break;

                    Console.WriteLine("Insufficient payment.");
                }

                Console.WriteLine("Change: PHP" + (payment - netTotal));
                receiptNo++;

                //clear cart
               
                
                // Save order to history
                orderHistory[orderCount] = netTotal;
                orderCount++;

                //LOW STOCK ALERT
                Console.WriteLine("\nLOW STOCK ALERT:");
                for (int i = 0; i < products.Length; i++)                {
                    if (products[i].Stock <= 5)
                    {
                        Console.WriteLine(products[i].Name + " has only " + products[i].Stock + " left in stock.");
                    }
                }
                    Console.WriteLine("\nThank you for shopping with us!");
            
            }
                // ============== ORDER HISTORY ==============
            else if (option == "5")
                {
                    Console.WriteLine("\n--- ORDER HISTORY ---");
                    if (orderCount == 0)
                    {
                        Console.WriteLine("No orders yet.");
                    }
                    else
                    {
                        for (int i = 0; i < orderCount; i++)
                    {
                        Console.WriteLine("Order " + (i + 1) + ": PHP" + orderHistory[i]);
                    }
                    }
                }
                // CONTINUE VALIDATION
                while (true)
                {
                    Console.Write("\nBack to main menu? (Y/N): ");
                    choice = (Console.ReadLine() ?? "").ToUpper();   
                    if (choice == "Y" || choice == "N") break;

                    Console.WriteLine("Invalid choice. Please enter Y or N.");
                }
            }
            Console.WriteLine("\nProgram terminated. Goodbye!");
        }   

                    

class Program : NewBaseType
{
}
}








