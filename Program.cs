using System;
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

    public Product (int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }

    public void DisplayProduct()
    {
        if (Stock != 0)
        {
            Console.WriteLine(Id + ". " + Name + " - PHP" + Price + " (Stock: " + Stock + ")");
        }
        else
        {
            Console.WriteLine(Id + ". " + Name + " - PHP" + Price + " (Out of stock)");
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
        Product[] products = new Product [7];
        products [0] = new Product(1, "Tshirt", 500, 5);
        products[1] = new Product(2,"Sweater", 600, 4);
        products[2]= new Product(3,"Hoodie", 750, 7);
        products[3] = new Product(4, "Jeans", 1000, 3);
        products[4] = new Product(5, "Shoes", 1500, 2);
        products[5] = new Product(6, "Closed cap", 300, 10);
        products[6] = new Product(7, "Jorts", 200, 8);

        int[] cartQty = new int[7]; 
        double grandTotal = 0;
        string choice = "Y";

        while (choice == "Y")
        {
            Console.WriteLine("\nWELCOME TO OUR STORE!");
            Console.WriteLine("\nSTORE MENU");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            Console.WriteLine("Enter your product number: ");
            int num;
            if (!int. TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid number.");
                continue;
            }

            if (num < 1   || num > products.Length)
            {
                Console.WriteLine("Wrong product number.");
                continue;
            }

            Product p = products[num - 1];
            
            if (p.Stock == 0)
            {
                Console.WriteLine("Sorry, " + p.Name + " is out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            int qty;
            if (!int.TryParse(Console.ReadLine(), out qty))
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (qty <= 0)
            {
                Console.WriteLine("Quantity must be more than 0.");
                continue;
            }

            if (qty > p.Stock)
            {
                Console.WriteLine("Not enough stock.");
                continue;
            }

            cartQty[num - 1] += qty;
            p.Stock -= qty;

            Console.WriteLine("Added to cart!");

            Console.Write("Add more? Y/N: ");
            

while (true)
{
    choice = (Console.ReadLine() ?? "").ToUpper();

    if (choice == "Y" || choice == "N")
        break;

    Console.WriteLine("Invalid input. Please enter Y or N only.");
}

if (choice == "N")
{
    break;
}
        }

        Console.WriteLine("\nRECEIPT");
        Console.WriteLine("================================");
        Console.WriteLine("       KYLE'S CLOTHING STORE       ");
        Console.WriteLine("================================");
        Console.WriteLine("receipt No: " + new Random().Next(1000, 9999));
        Console.WriteLine("Date: " + DateTime.Now);
        Console.WriteLine("================================\n");
    for (int i = 0; i < products.Length; i++)
    {
        if (cartQty[i] > 0)
        {
            double total = products[i].GetTotal(cartQty[i]);
            Console.WriteLine(products[i].Name + " X " + cartQty[i] + " = PHP" + total);
            grandTotal += total;
        }
    }

    Console.WriteLine("Grand Total: PHP" + grandTotal);

    double discount = 0;
    if (grandTotal >= 5000)
    {
        discount = grandTotal * 0.10;
    }

    Console.WriteLine("Discount: PHP" + discount);
    Console.WriteLine("Final Total: PHP" + (grandTotal - discount));

    Console.WriteLine("\nUPDATED STOCK");
    for (int i = 0; i < products.Length; i++)
    {
        Console.WriteLine(products[i].Name + " - " + products[i].Stock);
    }

    Console.WriteLine("\nThank you for shopping with us!");   
    Console.WriteLine("Have a nice day!"); 
}
}

            
    
        

        



    





