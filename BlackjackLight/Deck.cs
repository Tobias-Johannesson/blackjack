using System;
using System.Collections.Generic;
using System.Linq;

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
