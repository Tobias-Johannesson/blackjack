using System;

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int GetValue()
    {
        if (Rank == Rank.Ace)
        {
            // Return 11 if adding 11 won't cause the player to bust, otherwise return 1
            return 11; // Assuming the default value is 11 (it can be adjusted later)
        }
        else if (Rank >= Rank.Two && Rank <= Rank.Ten)
        {
            return (int)Rank;
        }
        else
        {
            // For face cards (Jack, Queen, King)
            return 10;
        }
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
