using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Transactions;

enum Suit {
    Spades,
    Clubs,
    Diamonds,
    Hearts
}

enum Rank {
    Ace = 1,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King
}

class Card {
    public Rank rank;
    public Suit suit;

    public Card(Rank rank, Suit suit) {
        this.rank = rank;
        this.suit = suit;
    }

    public override string ToString() {
        return $"{rank} of {suit}";
    }
}

class Deck {
    List<Card> deck = new List<Card>();

    public Deck() {
        for (int i = 1; i <= 13; i++) {
            for (int j = 0; j < 4; j++) {
                deck.Add(new Card((Rank) i, (Suit) j));
            }
        }
        Shuffle();
    }

    public void Shuffle() {
        Random r = new Random();
        // Start from the last element and
        // swap one by one. We don't need to
        // run for the first element 
        // that's why i > 0
        for (int i = 51; i > 0; i--) {

            // Pick a random index
            // from 0 to i
            int j = r.Next(0, i + 1);

            // Swap arr[i] with the
            // element at random index
            Card temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }

        //print out deck
        //for (int i = 0; i < 52; i++)
            //Console.Write(deck[i] + "\n");
    }


}

class Poker {

    static void Main(string[] args) {
        Console.WriteLine("Welcome to Akshit's Texas Hold 'Em Poker Game!");
        // Initialize game components here
        // For example, create a deck of cards, shuffle them, deal cards to players, etc.
        // Placeholder for game logic

        Console.WriteLine("Initializing game components...");

        Deck playingDeck = new Deck();



        PlayGame();
    }

    static (String handName, List<Card> cardsInHand) GetHand(List<Card> community) {
        
        
        /*need to add hole cards from player to list*/


        //sort cards
        community.Sort((a,b) => a.rank - b.rank);

        //print sorted list
        //for (int i = 0; i < community.Count; i++)
        //    Console.Write(community[i] + "\n");

        (String handName, List<Card> cardsInHand) currHand = ("N/A", new List<Card>());
        if (community.Count > 2) {
            //Royal Flush case

            //Straight Flush case

            //4 of a Kind case

            //Full House case

            //Flush case

            //Straight case

            //Three of a Kind case

            //Two Pair case
        }

        //Pair case
        if (currHand.handName == "N/A")
            //fill in

        //High Card case
        if (currHand.handName == "N/A")
            currHand = ("High Card", community.GetRange(community.Count - 1, 1));
        
        return currHand;
    }
    static void PlayGame() {
        // Implement the game logic here
        Console.WriteLine("Game is starting...");
        // Example: Deal cards, evaluate hands, determine winner, etc.

        //checkHand(new List<Card>() { new Card(Rank.Three, Suit.Spades), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Seven, Suit.Spades), new Card(Rank.Seven, Suit.Hearts)});
    }
}