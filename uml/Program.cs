// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

namespace BankSystem
{
  class Program
  {
    static void Main(string[] args)
    {
      Bank bank = new Bank();
      bool exit = false;

      while (!exit)
      {
        Console.Clear();
        Console.WriteLine("Welcome to the Bank System");
        Console.WriteLine("1. Create Account");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Exit");
        Console.Write("Enter your choice: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
          case 1:
            CreateAccount(bank);
            break;
          case 2:
            LogIn(bank);
            break;
          case 3:
            exit = true;
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ReadKey();
            break;
        }
      }
    }

    static void CreateAccount(Bank bank)
    {
      Console.Write("Enter your name: ");
      string name = Console.ReadLine();
      Console.Write("Enter your PIN: ");
      string pin = Console.ReadLine();
      Console.Write("Enter your initial deposit amount: ");
      decimal initialDeposit = decimal.Parse(Console.ReadLine());

      Account account = new Account(name, pin, initialDeposit);
      bank.AddAccount(account);

      Console.WriteLine("Account created successfully.");
      Console.WriteLine($"Your account number is: {account.AccountNumber}");
      Console.ReadKey();
    }

    static void LogIn(Bank bank)
    {
      Console.Write("Enter your account number: ");
      int accountNumber = int.Parse(Console.ReadLine());
      Console.Write("Enter your PIN: ");
      string pin = Console.ReadLine();

      Account account = bank.GetAccount(accountNumber);

      if (account == null || account.Pin != pin)
      {
        Console.WriteLine("Invalid account number or PIN. Please try again.");
        Console.ReadKey();
        return;
      }

      bool loggedIn = true;

      while (loggedIn)
      {
        Console.Clear();
        Console.WriteLine($"Welcome, {account.Name}");
        Console.WriteLine("1. Check Balance");
        Console.WriteLine("2. Withdraw Cash");
        Console.WriteLine("3. Deposit Cash");
        Console.WriteLine("4. Transfer Funds");
        Console.WriteLine("5. Change PIN");
        Console.WriteLine("6. Log Out");
        Console.Write("Enter your choice: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
          case 1:
            CheckBalance(account);
            break;
          case 2:
            WithdrawCash(account);
            break;
          case 3:
            DepositCash(account);
            break;
          case 4:
            TransferFunds(bank, account);
            break;
          case 5:
            ChangePin(account);
            break;
          case 6:
            loggedIn = false;
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ReadKey();
            break;
        }
      }
    }

    static void CheckBalance(Account account)
    {
      Console.WriteLine($"Your current balance is: {account.Balance}");
      Console.ReadKey();
    }

    static void WithdrawCash(Account account)
    {
      Console.Write("Enter the amount to withdraw: ");
      decimal amount = decimal.Parse(Console.ReadLine());

      if (account.Withdraw(amount))
      {
        Console.WriteLine("Cash withdrawal successful.");
      }
      else
      {
        Console.WriteLine("Insufficient balance or invalid amount.");
      }

      Console.ReadKey();
    }

    static void DepositCash(Account account)
    {
      Console.Write("Enter the amount to deposit: ");
      decimal amount = decimal.Parse(Console.ReadLine());

      account.Deposit(amount);

      Console.WriteLine("Cash deposit successful.");
      Console.ReadKey();
    }

    static void TransferFunds(Bank bank, Account account)
    {
      Console.Write("Enter the account number to transfer to: ");
      int toAccountNumber = int.Parse(Console.ReadLine());
      Account toAccount = bank.GetAccount(toAccountNumber);

      if (toAccount == null)
      {
        Console.WriteLine("Invalid account number.");
        Console.ReadKey();
        return;
      }

      Console.Write("Enter the amount to transfer: ");
      decimal amount = decimal.Parse(Console.ReadLine());

      if (account.Transfer(toAccount, amount))
      {
        Console.WriteLine("Fund transfer successful.");
      }
      else
      {
        Console.WriteLine("Insufficient balance or invalid amount.");
      }

      Console.ReadKey();
    }

    static void ChangePin(Account account)
    {
      Console.Write("Enter your current PIN: ");
      string currentPin = Console.ReadLine();

      if (currentPin != account.Pin)
      {
        Console.WriteLine("Invalid current PIN.");
        Console.ReadKey();
        return;
      }

      Console.Write("Enter your new PIN: ");
      string newPin = Console.ReadLine();

      account.ChangePin(newPin);

      Console.WriteLine("PIN changed successfully.");
      Console.ReadKey();
    }
  }

  public class Bank
  {
    private List<Account> accounts = new List<Account>();

    public void AddAccount(Account account)
    {
      accounts.Add(account);
    }

    public Account GetAccount(int accountNumber)
    {
      return accounts.Find(a => a.AccountNumber == accountNumber);
    }
  }

  public class Account
  {
    public int AccountNumber { get; private set; }
    public string Name { get; private set; }
    public string Pin { get; private set; }
    public decimal Balance { get; private set; }

    public Account(string name, string pin, decimal initialDeposit)
    {
      Name = name;
      Pin = pin;
      Balance = initialDeposit;
      AccountNumber = GenerateAccountNumber();
    }

    public bool Withdraw(decimal amount)
    {
      if (amount <= 0 || amount > Balance)
      {
        return false;
      }

      Balance -= amount;
      return true;
    }

    public void Deposit(decimal amount)
    {
      if (amount <= 0)
      {
        throw new ArgumentException("Invalid deposit amount.");
      }

      Balance += amount;
    }

    public bool Transfer(Account toAccount, decimal amount)
    {
      if (amount <= 0 || amount > Balance)
      {
        return false;
      }

      if (Withdraw(amount))
      {
        toAccount.Deposit(amount);
        return true;
      }

      return false;
    }

    public void ChangePin(string newPin)
    {
      if (newPin.Length < 4 || newPin.Length > 8)
      {
        throw new ArgumentException("Invalid PIN length.");
      }

      Pin = newPin;
    }

    private int GenerateAccountNumber()
    {
      Random random = new Random();
      return random.Next(100000, 999999);
    }
    
  }
}
