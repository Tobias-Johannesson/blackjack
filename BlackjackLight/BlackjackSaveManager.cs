using System;
using System.IO;

public class BlackjackSaveManager
{
    private static readonly string saveFilePath = "blackjack_save.txt";
    const int INITIAL_AMOUNT = 200;

    public static int GetPlayerAmount(string playerName)
    {
        if (!File.Exists(saveFilePath))
        {
            CreateNewSaveFile();
        }

        string[] lines = File.ReadAllLines(saveFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2 && parts[0].Trim().Equals(playerName))
            {
                if (int.TryParse(parts[1].Trim(), out int amount))
                {
                    Console.WriteLine($"Welcome back {playerName}, you have {amount}");
                    return amount;
                }
            }
        }

        Console.WriteLine($"Welcome {playerName}, you will be given {INITIAL_AMOUNT}");
        AddNewPlayer(playerName, INITIAL_AMOUNT);
        return INITIAL_AMOUNT;
    }

    private static void AddNewPlayer(string playerName, int amount)
    {
        using (StreamWriter writer = File.AppendText(saveFilePath))
        {
            writer.WriteLine($"{playerName}, {amount}");
        }
    }

    private static void CreateNewSaveFile()
    {
        using (StreamWriter writer = File.CreateText(saveFilePath))
        {
            writer.WriteLine("Player Name, Amount");
        }
    }
}
