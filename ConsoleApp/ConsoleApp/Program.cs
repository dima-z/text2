// Note: I'm using a maximum code line length of 100 characters.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Script.Serialization;

namespace ConsoleApp
{
  public class Program
  {
    private const string _lexemAdd = "add";
    private const string _lexemGet = "get";
    private const string _lexemExit = "exit";

    private static Dictionary<int, Transaction> _storage =
      new Dictionary<int, Transaction>() {
#if (DEBUG)
        { 1, new Transaction() { Id = 1, TransactionDate = DateTime.Now, Amount  = 100 } },
        { 2, new Transaction() { Id = 2, TransactionDate = DateTime.Now, Amount  = 200} },
        { 99, new Transaction() { Id = 99, TransactionDate = DateTime.Now, Amount  = 90000} }
#endif
      };

    public static void Main(string[] args) => HandleCommand();

    /// <summary>
    /// Request and process a user command via console input.
    /// </summary>
    private static void HandleCommand()
    {
      var command = HandleInput("Введите команду:");
      switch (command.ToLower())
      {
        case _lexemAdd:
          HandleCommandAdd();
          break;

        case _lexemGet:
          HandleCommandGet();
          break;

        case _lexemExit:
          Environment.Exit(0);
          break;

        default:
          Console.WriteLine("Команда не опознанна.");
          HandleCommand();
          break;
      }

      HandleCommand();
    }

    /// <summary>
    /// Processes a "add" user command. Adds transaction into storage.
    /// </summary>
    private static void HandleCommandAdd()
    {
      var id = GetInt("Введите Id:", "Введен некорректный идентификатор. Повторите ввод.");
      if (_storage.TryGetValue(id, out _))
      {
        Console.WriteLine("Транзакция таким id уже существует. Введите другой id.");
        HandleCommandAdd();
        return;
      }

      var transaction = new Transaction
      {
        Id = id,
        TransactionDate = GetDate("Введите дату:", "Введен некорректная дата. Повторите ввод."),
        Amount = GetDecimal("Введите сумму:", "Введен некорректная сумма. Повторите ввод."),
      };

      _storage.Add(transaction.Id, transaction);
      Console.WriteLine("[OK]");
    }

    /// <summary>
    /// Checks input string for null or empty.
    /// </summary>
    /// <param name="message">Lable message</param>
    /// <param name="error">Error message</param>
    /// <returns>Checked input</returns>
    private static string HandleInput(string lable, string error = null)
    {
      if (error != null)
      {
        Console.WriteLine(error);
      }

      Console.WriteLine(lable);
      var property = Console.ReadLine().Trim();
      if (string.IsNullOrEmpty(property))
      {
        return HandleInput(lable, "Введены некорректные данный. Повторите ввод.");
      }

      return property;
    }

    /// <summary>
    /// Checks input string for integer value.
    /// </summary>
    /// <param name="message">Lable message</param>
    /// <param name="error">Error message</param>
    /// <param name="errorOccured">Flag, if need to show error</param>
    /// <returns></returns>
    private static int GetInt(string lable, string error, bool errorOccured = false)
    {
      var number = HandleInput(lable, errorOccured ? error : null);
      if (int.TryParse(number, out var intVal))
      {
        return intVal;
      }
      else
      {
        return GetInt(lable, error, true);
      }
    }

    /// <summary>
    /// Checks and parse input string as decimal value.
    /// </summary>
    /// <param name="message">Lable message</param>
    /// <param name="error">Error message</param>
    /// <param name="errorOccured">Flag, if need to show error</param>
    /// <returns></returns>
    private static decimal GetDecimal(string lable, string error, bool errorOccured = false)
    {
      var number = HandleInput(lable, errorOccured ? error : null);
      if (decimal.TryParse(number, out var decimalVal))
      {
        return decimalVal;
      }
      else
      {
        return GetDecimal(lable, error, true);
      }
    }

    /// <summary>
    /// Checks and parse input string as DateTime value.
    /// </summary>
    /// <param name="message">Lable message</param>
    /// <param name="error">Error message</param>
    /// <param name="errorOccured">Flag, if need to show error</param>
    /// <returns></returns>
    private static DateTime GetDate(string lable, string error, bool errorOccured = false)
    {
      var date = HandleInput(lable, errorOccured ? error : null);
      try
      {
        return DateTime.ParseExact(
          date,
          "d.M.yyyy",
          CultureInfo.InvariantCulture,
          DateTimeStyles.None
        );
      }
      catch (Exception)
      {
        return GetDate(lable, error, true);
      }
    }

    /// <summary>
    /// Searches for a transaction in storage and log result into console as JSON string.
    /// </summary>
    private static void HandleCommandGet()
    {
      var id = GetInt("Введите Id:", "Введен некорректный идентификатор. Повторите ввод.");
      if (_storage.TryGetValue(id, out var transaction))
      {
        Console.WriteLine(
          new JavaScriptSerializer().Serialize(new
          {
            id = transaction.Id,
            transactionDate = transaction
              .TransactionDate
              .ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"),
            amount = transaction.Amount
          })
        );
      }
      else
      {
        Console.WriteLine("Транзакция с таким id не найдена.");
      }
    }
  }
}
