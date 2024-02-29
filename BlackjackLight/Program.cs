using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 10,
    Queen = 10,
    King = 10
}

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}

public class Deck
{
    private List<Card> cards;
    private Random random;

    public Deck()
    {
        cards = GenerateDeck();
        random = new Random();
    }

    private List<Card> GenerateDeck()
    {
        var deck = new List<Card>();
        for (int i = 0; i < 4; i++) // Loop four times to create four decks
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    deck.Add(new Card(suit, rank));
                }
            }
        }
        return deck;
    }

    public void ReShuffle()
    {
        cards.Clear(); // Clear the current deck
        cards = GenerateDeck(); // Generate a new deck with all cards
        cards = cards.OrderBy(card => random.Next()).ToList(); // Shuffle the new deck
    }

    public Card Draw()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("The deck is empty.");
        }
        Card topCard = cards[0];
        cards.RemoveAt(0);
        return topCard;
    }

    public int Count()
    {
        return cards.Count;
    }
}

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

class Program
{
    const int RESHUFFLE_THRESHOLD = 52;

    static void Main(string[] args)
    {
        printLogo();
        string playerName = getUsername();
        int playerMoney = BlackjackSaveManager.GetPlayerAmount(playerName);
        playGame();
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
        string username = null;
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
        Deck deck = new Deck();
        deck.ReShuffle();
        Console.WriteLine("Let's start, dealer is shuffling the deck...");
        System.Threading.Thread.Sleep(3000);
        Console.WriteLine("Deck shuffled.");

        int[] dealerCards = new int[10];
        int[] playerCards = new int[10];
        int betSize = 10;

        Card topCard = deck.Draw();
        Console.WriteLine($"Top card drawn: {topCard}");
        System.Threading.Thread.Sleep(3000);

        if (deck.Count() < RESHUFFLE_THRESHOLD)
        {
            deck.ReShuffle();
            Console.WriteLine("Deck is reshuffled.");
        }
    }
}
