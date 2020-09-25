using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneConsole
{
    public class Program
    {

        private const int Pos_itemNumber = 0;
        private const int Pos_ItemName = 1;
        private const int Pos_ItemQuantity = 2;
        private const int Pos_itemPrice = 3;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("WELCOME TO THE BEST VENDING MACHINE EVER!!!! (Distant crowd roar)");
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Main Menu");
                Console.WriteLine("inv] Inventory");
                Console.WriteLine("order <amount> <item_number> <quantity>] Order");
                Console.WriteLine();

                string input = Console.ReadLine();

                List<InventoryItem> items = new List<InventoryItem>();
                string file = "Inventory.csv";
                //read inventory from inventory.csv and write on console
                if (File.Exists(file))
                {

                    try
                    {
                        using (StreamReader sr = new StreamReader(file))
                        {
                            while (!sr.EndOfStream)
                            {
                                // Read the line
                                string line = sr.ReadLine();

                                string[] itemDetails = line.Split("|");

                                if (!decimal.TryParse(itemDetails[Pos_itemPrice], out decimal itemPrice))
                                {
                                    itemPrice = 0M;
                                }

                                InventoryItem item = new InventoryItem();
                                item.Id = Convert.ToInt32(itemDetails[Pos_itemNumber]);
                                item.Quantity = Convert.ToInt32(itemDetails[Pos_ItemQuantity]);
                                item.ItemName = itemDetails[Pos_ItemName];
                                item.Price = itemPrice;
                                items.Add(item);
                            }
                        }


                    }
                    catch
                    {
                        Console.WriteLine("Error trying to open the inventory file.");
                    }
                }
                else
                {
                    Console.WriteLine("Input file is missing!! The vending machine will now self destruct.");
                }

                if (input.ToLower() == "inv")
                {
                    await ViewInventory(items);
                }
                Console.WriteLine();
                if (input.ToLower().Contains("order"))
                {
                    await OrderConfirmation(items,input);
                }
            }

        }

        public static async Task ViewInventory(List<InventoryItem> items)
        {
            if (items.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Current Inventory");
                foreach (var product in items)
                {
                    Console.WriteLine(product.Id + " " + product.ItemName + " (" + product.Quantity + "): $" + product.Price);

                }
            }
        }

        public static async Task OrderConfirmation(List<InventoryItem> items,string input)
        {
            string[] orderInput = input.Split(" ");
            if (orderInput.Length == 4)
            {
                bool isSuccess = true;
                if (!decimal.TryParse(orderInput[1], out decimal amount))
                {
                    amount = 0M;
                }
                if (!int.TryParse(orderInput[2], out int itemNumber))
                {
                    itemNumber = 0;
                }
                if (!int.TryParse(orderInput[3], out int qty))
                {
                    qty = 0;
                }

                if (amount == 0)
                {
                    Console.WriteLine("Order is not Successful - Incorrect Amount");
                    isSuccess = false;
                }
                if (itemNumber == 0 || items.Where(c => c.Id == itemNumber).Count() <= 0)
                {
                    Console.WriteLine("Order is not Successful - Incorrect Item Number");
                    isSuccess = false;
                }
                if (qty == 0)
                {
                    Console.WriteLine("Order is not Successful - Incorrect Quantity");
                    isSuccess = false;
                }

                if (isSuccess)
                {
                    //get item amount 
                    decimal actualPrice = items.Where(c => c.Id == itemNumber).FirstOrDefault().Price;
                    decimal actualQty = items.Where(c => c.Id == itemNumber).FirstOrDefault().Quantity;

                    int calculatedQty = (int)(amount / actualPrice);
                    if (qty == calculatedQty && qty<=actualQty)
                    {
                        Console.WriteLine("Order is Successful");
                    }
                    else if(qty > actualQty)
                    {
                        Console.WriteLine("Order is not Successful - Incorrect Quantity");
                    }
                    else
                    {
                        Console.WriteLine("Order is not Successful - Incorrect Amount");
                    }

                }
                Console.WriteLine();
            }

            else
            {
                Console.WriteLine("Invalid Input");
            }
        }
    }
}
