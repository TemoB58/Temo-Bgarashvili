using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Banking_Application
{
    internal class Program
    {
        public static List<Person> people;
        private static Person currentUser;
        private const string JsonFilePath = "C:\\Users\\Tinatin Supatashvili\\Desktop\\Banking Application - final project\\Banking Application\\People.json";
        



        static void Main(string[] args)
        {
            LoadDataFromJson();
            foreach(var person in people)
            {

                Console.WriteLine(person.FirstName);
            }
            Console.WriteLine();

            Console.WriteLine("Welcome to the ATM Simulator!");
            Console.WriteLine("Please enter your card number:");
            string inputCardNumber = Console.ReadLine()!;

            currentUser = people.SingleOrDefault(p => p.CardNumber == inputCardNumber)!;
            if (currentUser == null)
            {
                Console.WriteLine("Invalid card number. Exiting.");
                return;
            }

            Console.WriteLine("Please enter your exprationDate(ex: 05/26): ");
            string expirationDate = Console.ReadLine()!;

            if (expirationDate != currentUser.ExpirationDate)
            {
                Console.WriteLine("Incorrect expirationDate. Exiting.");
                return;
            }

            Console.WriteLine("Please enter your PIN:");
            string inputPin = Console.ReadLine()!;

            if (inputPin != currentUser.PinCode)
            {
                Console.WriteLine("Incorrect PIN. Exiting.");
                return;
            }

            try
            {
                while (true)
                {
                    Console.WriteLine("\nChoose an action:");
                    Console.WriteLine("1. View balance");
                    Console.WriteLine("2. Withdraw money");
                    Console.WriteLine("3. Last 5 operations");
                    Console.WriteLine("4. Deposit money");
                    Console.WriteLine("5. Change PIN");
                    Console.WriteLine("6. Currency conversion");
                    Console.WriteLine("7. Exit");

                    string choice = Console.ReadLine()!;

                    try
                    {
                        switch (choice)
                        {
                            case "1":
                                ViewBalance();
                                break;
                            case "2":
                                WithdrawMoney();
                                break;
                            case "3":
                                LastFiveOperations();
                                break;
                            case "4":
                                DepositMoney();
                                break;
                            case "5":
                                ChangePIN();
                                break;
                            case "6":
                                CurrencyConversion();
                                break;
                            case "7":
                                Console.WriteLine("Thank you for using our ATM. Goodbye!");
                                return;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
            catch (Exception ex )
            {
                LogErrorIntoFile( ex );
            }
        }
    

        private static void LoadDataFromJson()
        {
            try
            {
                string json = File.ReadAllText(JsonFilePath);
                var data = JsonSerializer.Deserialize<JsonData>(json);

                if (data != null)
                {
                    people = data.People;
                }
                else
                {
                    Console.WriteLine("Error loading data from JSON.");
                    Environment.Exit(1);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("JSON file not found.");
                Environment.Exit(1);
            }
            catch (JsonException)
            {
                Console.WriteLine("Error parsing JSON.");
                Environment.Exit(1);
            }
        }

        private static void ViewBalance()
        {
            try
            {
                double balance = GetBalance();
                SaveTransaction(balance, "ViewBalance");
                Console.WriteLine($"Current balance: {balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving balance: {ex.Message}");
            }
        }

        private static void WithdrawMoney()
        {
            try
            {
                Console.WriteLine("Enter amount to withdraw:");
                double amount = Convert.ToDouble(Console.ReadLine());

                double balance = GetBalance();
                if (amount <= balance)
                {
                    balance -= amount;
                    SaveTransaction(amount, "Withdrawal");
                    Console.WriteLine($"Withdrawal successful. Remaining balance: {balance}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while withdrawing money: {ex.Message}");
            }
        }

        private static void LastFiveOperations()
        {
            try
            {
                List<Transaction> transactions = GetLastFiveTransactions();
                Console.WriteLine("Last 5 operations:");
                foreach (Transaction transaction in transactions)
                {
                    SaveTransaction(GetBalance(), "LastFiveOperations");
                    Console.WriteLine($"{transaction.AmountGel} - {transaction.TransactionDate} - {transaction.TransactionType}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving last five operations: {ex.Message}");
            }
        }

        private static void DepositMoney()
        {
            try
            {
                Console.WriteLine("Enter amount to deposit:");
                double amount = Convert.ToDouble(Console.ReadLine());

                double balance = GetBalance();
                balance += amount;
                SaveTransaction(amount, "Deposit");
                Console.WriteLine($"Deposit successful. New balance: {balance}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while depositing money: {ex.Message}");
            }
        }


        private static void CurrencyConversion()
        {
            try
            {
                Console.WriteLine("Enter amount to convert:");
                double amount = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Enter source currency (e.g., USD, EUR, GEL):");
                string sourceCurrency = Console.ReadLine().ToUpper();

                Console.WriteLine("Enter target currency (e.g., USD, EUR, GEL):");
                string targetCurrency = Console.ReadLine().ToUpper();

                
                double convertedAmount;

                if (sourceCurrency == "GEL")
                {
                    if (targetCurrency == "USD")
                        convertedAmount = amount / 2.68;
                    else if (targetCurrency == "EUR")
                        convertedAmount = amount / 3.11;
                    else
                        convertedAmount = amount;
                }
                else if (sourceCurrency == "USD")
                {
                    if (targetCurrency == "GEL")
                        convertedAmount = amount * 2.68;
                    else if (targetCurrency == "EUR")
                        convertedAmount = amount * 0.93;
                    else
                        convertedAmount = amount;
                }
                else if (sourceCurrency == "EUR")
                {
                    if (targetCurrency == "GEL")
                        convertedAmount = amount * 3.11;
                    else if (targetCurrency == "USD")
                        convertedAmount = amount * 1.08;
                    else
                        convertedAmount = amount;
                }
                else
                {
                    Console.WriteLine("Invalid source currency.");
                    return;
                }

                SaveTransaction(amount, "CurrencyConversion");
                Console.WriteLine($"Converted amount: {convertedAmount} {targetCurrency}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while performing currency conversion: {ex.Message}");
            }
        }


        private static void ChangePIN()
        {
            try
            {
                Console.WriteLine("Enter new PIN:");
                string newPin = Console.ReadLine();
                currentUser.PinCode = newPin;
                SaveDataToJson();
                SaveTransaction(GetBalance(), "ChangePin");
                Console.WriteLine("PIN changed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while changing PIN: {ex.Message}");
            }
        }

        private static double GetBalance()
        {
            try
            {
                double balance = 0;
                if (currentUser != null)
                {
                    foreach (Transaction transaction in currentUser.TransactionHistory)
                    {
                        if (transaction.TransactionType == "Deposit")
                        {
                            balance += transaction.AmountGel;
                        }
                        else if (transaction.TransactionType == "Withdrawal")
                        {
                            balance -= transaction.AmountGel;
                        }
                    }
                }
                return balance;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Transaction file not found.");
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving balance.", ex);
            }
        }

        private static List<Transaction> GetLastFiveTransactions()
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                if (currentUser != null)
                {
                    transactions = currentUser.TransactionHistory;
                    transactions.Reverse();
                    transactions = transactions.GetRange(0, Math.Min(5, transactions.Count));
                }
                return transactions;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Transaction file not found.");
                return new List<Transaction>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving last five transactions.", ex);
            }
        }

        private static void SaveTransaction(double amountGel, string transactionType)
        {
            try
            {
                Transaction transaction = new Transaction
                {
                    TransactionDate = DateTime.Now.ToString("yyyy-MM-dd-hh-mm"),
                    TransactionType = transactionType,
                    AmountGel = amountGel,
                    AmountUSD = amountGel / 2.68, 
                    AmountEUR = amountGel / 3.11 
                };

                if (currentUser != null)
                {
                    if (currentUser.TransactionHistory == null)
                    {
                        currentUser.TransactionHistory = new List<Transaction>();
                    }

                    currentUser.TransactionHistory.Add(transaction);

                    SaveDataToJson();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving transaction.", ex);
            }
        }

        private static void SaveDataToJson()
        {
            try
            {
                var data = new JsonData { People = people };
                string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(JsonFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving data to JSON.", ex);
            }
        }

        private static void LogErrorIntoFile(Exception ex)
        {
            string path = "C:\\Users\\Tinatin Supatashvili\\Desktop\\Banking Application - final project\\Banking Application\\ErrorLogs.txt";
            File.WriteAllText(path, ex.Message);
            File.WriteAllText(path, "\n");
        }


    }
}
