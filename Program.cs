using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LoxNet.Scan;
using LoxNet.Tokens;


namespace LoxNet
{
  class Program
  {
    static bool hadError = false;

    static void Main(string[] args)
    {
      if (args.Length > 1)
      {
        Console.WriteLine("Usage LoxNet [script]");
      }
      else if (args.Length == 1)
      {
        runFile(args[0]);
      }
      else
      {
        runPrompt();
      }
    }

    private static void runFile(string path)
    {
      string s = File.ReadAllText(path);
      run(s);

        // Indicate an error in the exit code.
      if (hadError)
        Environment.Exit(64);
    }

    private static void runPrompt()
    {
      for (;;)
      {
        Console.Write("> ");
        string line = Console.ReadLine();

        if (line == null)
          break;

        run(line);
        hadError = false;
      }
    }


    private static void run(string source)
    {
      Scanner scanner = new Scanner(source);
      List<Token> tokens = scanner.scanTokens();

      //var tokens = source.Split(" ");

      foreach (var token in tokens)
      {
        Console.WriteLine(token);
      }
    }

    public static void error(int line, string message)
    {
      report(line, "", message);
    }

    private static void report(int line, string where, string message)
    {
      Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
      hadError = true;
    }

  }

}