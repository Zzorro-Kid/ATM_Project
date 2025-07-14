using System;
using System.Collections.Generic;
using System.IO;

namespace ATMTerminal
{
    public class ATM
    {
        private List<User> users = new List<User>();
        private User? currentUser = null;
        private readonly string dataFilePath;

        public ATM(string dataFilePath)
        {
            this.dataFilePath = dataFilePath;
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var lines = File.ReadAllLines(dataFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(' ');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int pin))
                    {
                        users.Add(new User(parts[0], pin, 1000)); // Default balance 1000
                    }
                }

                if (users.Count == 0)
                {
                    Console.WriteLine("No valid users found in the data file.");
                    Environment.Exit(1);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: Data file '{dataFilePath}' not found.");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
                Environment.Exit(1);
            }
        }

        public void Run()
        {
            if (AuthenticateUser())
            {
                ShowMainMenu();
            }
        }

        private bool AuthenticateUser()
        {
            for (int attempts = 3; attempts > 0; attempts--)
            {
                Console.Write("Enter PIN: ");
                if (int.TryParse(Console.ReadLine(), out int enteredPin))
                {
                    currentUser = users.Find(u => u.Pin == enteredPin);
                    if (currentUser != null)
                    {
                        Console.WriteLine($"\nWelcome, {currentUser.Name}!");
                        return true;
                    }
                }

                Console.WriteLine($"Wrong PIN! Attempts left: {attempts - 1}");
            }

            Console.WriteLine("\nAccess Denied. Too many attempts.");
            return false;
        }

        private void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== ATM Terminal ===");
                Console.WriteLine("1. Show balance");
                Console.WriteLine("2. Deposit money");
                Console.WriteLine("3. Withdraw money");
                Console.WriteLine("4. Exit");
                Console.Write("\nSelect an action: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ShowBalance();
                            break;
                        case 2:
                            Deposit();
                            break;
                        case 3:
                            Withdraw();
                            break;
                        case 4:
                            Console.WriteLine("\nThank you for using our ATM. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("\nInvalid choice! Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nPlease enter a valid number.");
                }
            }
        }

        private void ShowBalance()
        {
            Console.WriteLine($"\nYour balance: ${currentUser?.Balance}");
        }

        private void Deposit()
        {
            Console.Write("\nEnter the deposit amount: $");
            if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
            {
                currentUser?.Deposit(amount);
                Console.WriteLine($"\n${amount} successfully deposited!");
                Console.WriteLine($"New Balance: ${currentUser?.Balance}");
            }
            else
            {
                Console.WriteLine("\nInvalid amount! Please enter a positive number.");
            }
        }

        private void Withdraw()
        {
            Console.Write("\nEnter the withdrawal amount: $");
            if (int.TryParse(Console.ReadLine(), out int amount))
            {
                if (currentUser?.Withdraw(amount) == true)
                {
                    Console.WriteLine($"\nYou withdrew: ${amount}");
                    Console.WriteLine($"New Balance: ${currentUser?.Balance}");
                }
                else
                {
                    Console.WriteLine("\nInsufficient funds or invalid amount!");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid amount! Please enter a positive number.");
            }
        }
    }
}