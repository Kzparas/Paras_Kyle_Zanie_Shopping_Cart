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
        if (Stock != 0)
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

class Program
{
    static void Main()
    {
        Product[] products = [
            new Product(1, "Tshirt", "Topwear", 500, 5),
            new Product(2, "Sweater", "topwear", 600, 4),
            new Product(3, "Hoodie", "Topwear", 750, 7),
            new Product(4, "Jeans", "Bottomwear", 1200, 3),
            new Product(5, "Shoes", "Footwear", 1500, 2),
            new Product(6, "Closed cap", "Accessories", 300, 10),
            new Product(7, "Jorts", "Bottomwear", 200, 8)
        ];

        int[] cartQty = new int[7];
        string choice = "Y";

        while (choice == "Y")
        {
            Console.WriteLine("\n====== MAIN MENU ======");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Checkout");
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
                Console.WriteLine("3. Back");
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

                    int currentQty = cartQty[id - 1];
                    products[id - 1].Stock += currentQty;

                    if (products[id - 1].HasenoughStock(newQty))
                    {
                        cartQty[id - 1] = newQty;
                        products[id - 1].deductStock(newQty);

                        Console.WriteLine("Cart updated.");
                    }
                    else
                    {
                        products[id - 1].deductStock(currentQty);
                        Console.WriteLine("Not enough stock.");
                    }
                }
            }
            // ============== CHECKOUT ==============
            else if (option == "4")
            {
                double grandtotal = 0;
                Console.WriteLine("\n--- RECEIPT ---");

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
                double nettotal = grandtotal - discount;

                Console.WriteLine("\nTotal: PHP" + grandtotal);
                Console.WriteLine("Discount: PHP" + discount);
                Console.WriteLine("Final: PHP " + nettotal);

                double payment;
                while (true)
                {
                    Console.Write("Enter payment amount: ");
                    if (!double.TryParse(Console.ReadLine(), out payment)) continue;

                    if (payment >= nettotal) break;
                    Console.WriteLine("Insufficient payment.");
                }

                Console.WriteLine("Change: PHP" + (payment - nettotal));

                Console.WriteLine("\nThank you for shopping with us!");
                break;
            }

            Console.Write("\nGo back to main menu? (Y/N): ");
            choice = Console.ReadLine().ToUpper();
        }
    }
}








               