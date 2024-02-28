using System;
using System.Reflection.Metadata.Ecma335;
using System.IO;

public class BlackjackSaveManager
{
    private static readonly string saveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "blackjack_save.txt");
    const int INITIAL_AMOUNT = 200;

    // Function to get the amount of money for a player
    public static int GetPlayerAmount(string playerName)
    {
        // Check if the save file exists
        if (!File.Exists(saveFilePath))
        {
            // Create a new save file if it doesn't exist
            CreateNewSaveFile();
            return 1000; // Default starting amount
        }

        // Read the save file and find the player's amount
        string[] lines = File.ReadAllLines(saveFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2 && parts[0].Trim().Equals(playerName))
            {
                if (int.TryParse(parts[1].Trim(), out int amount))
                {
                    Console.WriteLine("Welcome back " + playerName + " you have " + amount);
                    return amount;
                }
            }
        }

        Console.WriteLine("Welcome " + playerName + " you will be given " + INITIAL_AMOUNT);

        // If player not found, add them with default amount
        AddNewPlayer(playerName, INITIAL_AMOUNT);
        return INITIAL_AMOUNT; // Default starting amount
    }

    // Function to add a new player to the save file
    private static void AddNewPlayer(string playerName, int amount)
    {
        using (StreamWriter writer = File.AppendText(saveFilePath))
        {
            writer.WriteLine($"{playerName}, {amount}");
        }
    }

    // Function to create a new save file with headers
    private static void CreateNewSaveFile()
    {
        using (StreamWriter writer = File.CreateText(saveFilePath))
        {
            writer.WriteLine("Player Name, Amount");
        }
    }
}

class Program
{
    static void Main(string[] args) 
    {
        // Starting the application
        int firstCard, secondCard, totalCardScore;
        int betSize = 10;

        printLogo();

        String playerName = getUsername();
        int playerMoney = BlackjackSaveManager.GetPlayerAmount(playerName);

        //playGame();

        //exitGame();
    }

    static void printLogo()
    {
        Console.Title = "Blackjack CLI";
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("    ______ _            _    _            _      _     _       _     _                ");
        Console.WriteLine("    | ___ \\ |          | |  (_)          | |    | |   (_)     | |   | |              ");
        Console.WriteLine("    | |_/ / | __ _  ___| | ___  __ _  ___| | __ | |    _  __ _| |__ | |_              ");
        Console.WriteLine("    | ___ \\ |/ _` |/ __| |/ / |/ _` |/ __| |/ / | |   | |/ _` | '_ \\| __|           ");
        Console.WriteLine("    | |_/ / | (_| | (__|   <| | (_| | (__|   <  | |___| | (_| | | | | |_              ");
        Console.WriteLine("    \\____/|_|\\__,_|\\___|_|\\_\\ |\\__,_|\\___|_|\\_\\ \\_____/_|\\__, |_| |_|\\__| ");
        Console.WriteLine("                           _/ |                           __/ |     ");
        Console.WriteLine("                          |__/                           |___/");
        Console.WriteLine("");
    }

    static string getUsername()
    {
        String username = null;
        while (username == null)
        {
            Console.WriteLine("Who is trying to play?");
            username = Console.ReadLine();
            Console.Clear();
        }

        return username;
    }

    static void playGame()
    {
        Console.WriteLine("Let's start");

        // ROUNDS

        // Exit
    }

    static void exitGame()
    {
        Console.Clear();
        Console.WriteLine("Thanks for playing, hope to see you soon!");
    }
}










     
    
