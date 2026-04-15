using System;
using System.Data.Common;

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
        price = price;
        stock = stock;
    }

    public static void Main()
    {
        Console.Writeline(Id + ". " + Name + " - ₱" + Price + " |  Stock: " + stock);
    }
    public double GetTotal(int qty)
    { 
        return Predicate * qty;
    }
}

class program
{ 
    static void Main()
    {
        Product[] products = new Product [3];
        products [0] = new Product(1, "Tshirt", 500, 5);
        products[1] = new products(2,"Sweater", 600, 4);
        products[2= new products(3,"Hoodie", 750, 7);

        int[] cartQty = new int[3]; 
        double grandTotal = 0;
        string choice = "Y";

        while (choice == "Y")
        {
            Console.WriteLine("\STORE MENU");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

        



    





