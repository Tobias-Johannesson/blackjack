using System;

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

    static int CalculateHandScore(Card[] cards, int cardCount)
    {
        int score = 0;
        int aceCount = 0; // Count the number of Aces in the hand

        // Iterate through each card in the hand
        for (int i = 0; i < cardCount; i++)
        {
            int cardValue = cards[i].GetValue();

            if (cardValue == 1) // If the card is an Ace
            {
                cardValue = 11; // Default to 11
                aceCount++;
            } else if (cardValue > 10)
            {
                cardValue = 10; // Count high cards as 10
            }

            score += cardValue;
        }

        // Adjust the score for Aces
        while (aceCount > 0 && score > 21)
        {
            score -= 10; // Treat the Ace as 1 instead of 11
            aceCount--;
        }

        return score;
    }

    static void PrintHand(Card[] cards, int cardCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            Console.Write("+-----+");
        }
        Console.WriteLine();

        for (int i = 0; i < cardCount; i++)
        {
            Console.Write($"| {GetCardSymbol(cards[i])}   |");
        }
        Console.WriteLine();

        for (int i = 0; i < cardCount; i++)
        {
            Console.Write("|     |");
        }
        Console.WriteLine();

        for (int i = 0; i < cardCount; i++)
        {
            Console.Write($"|   {GetCardSymbol(cards[i])} |");
        }
        Console.WriteLine();

        for (int i = 0; i < cardCount; i++)
        {
            Console.Write("+-----+");
        }
        Console.WriteLine();
    }

    static string GetCardSymbol(Card card)
    {
        switch (card.Rank)
        {
            case Rank.Ace: return "A";
            case Rank.Two: return "2";
            case Rank.Three: return "3";
            case Rank.Four: return "4";
            case Rank.Five: return "5";
            case Rank.Six: return "6";
            case Rank.Seven: return "7";
            case Rank.Eight: return "8";
            case Rank.Nine: return "9";
            case Rank.Ten: return "10";
            case Rank.Jack: return "J";
            case Rank.Queen: return "Q";
            case Rank.King: return "K";
            default: return "?";
        }
    }

    static void playGame()
    {
        Deck deck = new Deck();
        bool isPlaying = true;

        while (isPlaying)
        {
            // Check if user has enough money to play, subtract amount
            // Else isPlaying = False, break

            if (deck.Count() < RESHUFFLE_THRESHOLD)
            {
                deck.ReShuffle();
                Console.WriteLine("Let's start, dealer is shuffling the deck...");
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine("Deck shuffled.");
            }

            // Start a new round
            int dealerCardCount = 0;
            int playerCardCount = 0;
            Card[] dealerCards = new Card[10];
            Card[] playerCards = new Card[10];
            int betSize = 10;

            // Dealer draws first card
            Card topCard = deck.Draw();
            dealerCards[dealerCardCount] = topCard;
            dealerCardCount++;
            Console.WriteLine("Dealer draws a card...");

            // Player draws first card
            topCard = deck.Draw();
            playerCards[playerCardCount] = topCard;
            playerCardCount++;
            Console.WriteLine($"Player got: {topCard}");

            // Dealer draws second card
            topCard = deck.Draw();
            dealerCards[dealerCardCount] = topCard;
            dealerCardCount++;
            Console.WriteLine($"Dealer got: {topCard}");

            // Player draws second card
            topCard = deck.Draw();
            playerCards[playerCardCount] = topCard;
            playerCardCount++;
            Console.WriteLine($"Player got: {topCard}");

            // Player's turn
            while (true)
            {
                // Print status message with dealer's hand and player's hand
                Console.Clear();
                PrintHand(dealerCards, dealerCardCount);
                PrintHand(playerCards, playerCardCount);

                Console.WriteLine("What will you do?");
                Console.WriteLine("1. Hit");
                Console.WriteLine("2. Stand");
                // Console.WriteLine("3. Side actions");
                int userAction = Convert.ToInt32(Console.ReadLine());

                if (userAction == 1)
                {
                    // Player hits
                    topCard = deck.Draw();
                    playerCards[playerCardCount] = topCard;
                    playerCardCount++;
                    Console.WriteLine($"Player got: {topCard}");

                    // Check if player busts
                    if (CalculateHandScore(playerCards, playerCardCount) > 21)
                    {
                        break;
                    }
                }
                else if (userAction == 2)
                {
                    // Player stands
                    break;
                }
                else if (userAction == 3)
                {
                    // Player wants a side action
                    break;
                }
            } // Player turn done

            // Calculate player's score
            int playerScore = CalculateHandScore(playerCards, playerCardCount);

            // Dealer's turn
            while (CalculateHandScore(dealerCards, dealerCardCount) < 17)
            {
                // Dealer draws a card
                topCard = deck.Draw();
                dealerCards[dealerCardCount] = topCard;
                dealerCardCount++;
                Console.WriteLine($"Dealer draws: {topCard}");
            }

            // Calculate dealer's score
            int dealerScore = CalculateHandScore(dealerCards, dealerCardCount);

            // Determine winner
            if (playerScore > 21)
            {
                Console.WriteLine("Player busts, dealer wins!");
            }
            else if (dealerScore > 21 || playerScore > dealerScore)
            {
                Console.WriteLine("Player wins!");
                // Give user money back, twice the bet size
            }
            else if (playerScore < dealerScore)
            {
                Console.WriteLine("Dealer wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
                // Give money back on a push, one bet size
            }

            // Option to play another round
            Console.WriteLine("Do you want to play another round? (Y/N)");
            string input = Console.ReadLine();
            isPlaying = (input.ToLower() == "y");
        }

        Console.WriteLine($"Welcome back anytime");
        // Save updated sum to file
        System.Threading.Thread.Sleep(1000);
    }
}
