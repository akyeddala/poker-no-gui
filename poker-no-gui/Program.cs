using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Transactions;

namespace PokerGame {

    public enum Suit {
        Spades,
        Clubs,
        Diamonds,
        Hearts
    }

    public enum Rank {
        Two = 2,
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
        King,
        Ace
    }

    public class Card {
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

    public enum Hands {
        None,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }

    public class Deck {
        private List<Card> deck = new List<Card>();

        public Deck() {
            for (int i = 2; i <= 14; i++) {
                for (int j = 0; j < 4; j++) {
                    deck.Add(new Card((Rank)i, (Suit)j));
                }
            }
            Shuffle();
        }

        public void Shuffle() {
            Random r = new Random();
            //Fisher-Yates Shuffle
            for (int i = 51; i > 0; i--) {

                //random index from 0 to i
                int j = r.Next(0, i + 1);

                // Swap deck[i] with element at random index
                Card temp = deck[i];
                deck[i] = deck[j];
                deck[j] = temp;
            }

            //print out deck
            //for (int i = 0; i < 52; i++)
            //Console.Write(deck[i] + "\n");
        }

        public Card Pop() {
            Card temp = (Card) deck[0];
            deck.RemoveAt(0);
            return temp;
        }
    }

    public class Hand {
        public Hands Name { get; set; }
        public List<Card> Cards { get; }
        public List<Card>? Kicker { get; }

        public Hand(Hands name, List<Card> cards, List<Card>? kicker = null) {
            this.Name = name;
            this.Cards = new List<Card>(cards);
            this.Kicker = kicker;
        }
    }

    public class Poker {

        static void Main(string[] args) {
            Console.WriteLine("Welcome to Akshit's Texas Hold 'Em Poker Game!");
            // Initialize game components here
            // For example, create a deck of cards, shuffle them, deal cards to players, etc.
            // Placeholder for game logic

            Console.WriteLine("Initializing game components...");

            PlayGame();
        }
        public static Hand GetHand(List<Card> cards) {

            List<Card> sortedCards = new List<Card>(cards);
            sortedCards.Sort((a, b) => b.rank - a.rank);
            Hand? resultHand = null;

            if (sortedCards.Count >= 5) {
                resultHand = getBestRoyalFlush(sortedCards);
                if (resultHand != null)
                    return resultHand;
                resultHand = getBestStraightFlush(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }
            if (sortedCards.Count >= 4) {
                resultHand = getBestFourOfAKind(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }
            if (sortedCards.Count >= 5) {
                resultHand = getBestFullHouse(sortedCards);
                if (resultHand != null)
                    return resultHand;
                resultHand = getBestFlush(sortedCards);
                if (resultHand != null)
                    return resultHand;
                resultHand = getBestStraight(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }

            if (sortedCards.Count >= 3) {
                resultHand = getBestThreeOfAKind(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }

            if (sortedCards.Count >= 4) {
                resultHand = getBestTwoPair(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }

            if (sortedCards.Count >= 2) {
                resultHand = getBestPair(sortedCards);
                if (resultHand != null)
                    return resultHand;
            }
           
            resultHand = getBestHighCard(sortedCards);
            return resultHand;






            static Hand? getBestRoyalFlush(List<Card> checkCards) {
                Hand? bestRF = getBestStraightFlush(checkCards);
                if (bestRF != null && bestRF.Cards[0].rank == Rank.Ace) {
                    bestRF.Name = Hands.RoyalFlush;
                    return bestRF;
                }
                return null;
            }


            static Hand? getBestStraightFlush(List<Card> checkCards) {
                List<Card> checkStraightFlush = new List<Card>(checkCards);
                checkStraightFlush.Sort((a, b) => a.suit - b.suit);
                for (int i = 0; i < checkStraightFlush.Count - 4; i++) {
                    List<Card> flush = new List<Card>() { checkStraightFlush[i] };
                    Suit currSuit = checkStraightFlush[0].suit;
                    for (int j = i + 1; j < checkStraightFlush.Count; j++) {
                        if (checkStraightFlush[j].suit == currSuit)
                            flush.Add(checkStraightFlush[j]);
                    }
                    if (flush.Count >= 5) {
                        Hand? result = getBestStraight(flush);
                        if (result != null) {
                            result.Name = Hands.StraightFlush;
                            return result;
                        }
                    }
                }
                return null;
            }

            static Hand? getBestFourOfAKind(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 3; i++) {
                    if (checkCards[i].rank == checkCards[i + 1].rank && checkCards[i].rank == checkCards[i + 2].rank && checkCards[i].rank == checkCards[i + 3].rank) {
                        List<Card> pair = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2] };
                        List<Card>? kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);
                        kickers.Remove(checkCards[i + 3]);
                        if (kickers.Count == 0) {
                            kickers = null;
                        }
                        else {
                            kickers = kickers.GetRange(0, 1);
                        }
                        return new Hand(Hands.FourOfAKind, pair, kickers);
                    }
                }
                return null;
            }

            static Hand? getBestFullHouse(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 2; i++) {
                    if (checkCards[i].rank == checkCards[i + 1].rank && checkCards[i].rank == checkCards[i + 2].rank) {
                        List<Card> full = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2] };
                        List<Card>? kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);

                        for (int j = 0; j < kickers.Count - 1; j++) {
                            if (checkCards[j].rank == checkCards[j + 1].rank) {
                                full.Add(checkCards[i]);
                                full.Add(checkCards[i + 1]);
                                return new Hand(Hands.FullHouse, full);
                            }
                        }
                    }
                }
                return null;
            }

            static Hand? getBestFlush(List<Card> checkCards) {
                List<Card> checkFlush = new List<Card>(checkCards);
                checkFlush.Sort((a, b) => a.suit - b.suit);
                for (int i = 0; i < checkFlush.Count - 4; i++) {
                    List<Card> flush = new List<Card>() { checkFlush[i] };
                    Suit currSuit = checkFlush[0].suit;
                    for (int j = i + 1; j < i + 4; j++) {
                        if (checkFlush[j].suit == currSuit)
                            flush.Add(checkFlush[j]);
                    }
                    if (flush.Count == 5)
                        return new Hand(Hands.Flush, flush);
                }
                return null;
            }

            static Hand? getBestStraight(List<Card> checkCards) {
                List<Card> checkStraight = new List<Card>(checkCards);
                for (int i = 0; i < checkCards.Count; i++) {
                    if (checkCards[i].rank == Rank.Ace)
                        checkStraight.Add(checkCards[i]);
                    else
                        break;
                }

                for (int i = 0; i < checkStraight.Count - 4; i++) {
                    List<Card> straight = new List<Card>() { checkStraight[i] };
                    Rank lastRank = checkStraight[i].rank;
                    for (int j = i + 1; j < checkStraight.Count; j++) {
                        if (checkStraight[j].rank == lastRank)
                            continue;
                        else if (checkStraight[j].rank == lastRank - 1 || checkStraight[j].rank == lastRank + 12) {
                            straight.Add(checkStraight[j]);
                            lastRank = checkStraight[j].rank;
                        }
                        else
                            break;
                    }
                    if (straight.Count >= 5)
                        return new Hand(Hands.Straight, straight.GetRange(0, 5));
                }
                return null;

            }

            static Hand? getBestThreeOfAKind(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 2; i++) {
                    if (checkCards[i].rank == checkCards[i + 1].rank && checkCards[i].rank == checkCards[i + 2].rank) {
                        List<Card> three = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2] };
                        List<Card>? kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);
                        if (kickers.Count == 0) {
                            kickers = null;
                        }
                        else {
                            kickers = kickers.GetRange(0, kickers.Count >= 2 ? 2 : kickers.Count - 1);
                        }
                        return new Hand(Hands.ThreeOfAKind, three, kickers);
                    }
                }
                return null;
            }

            static Hand? getBestTwoPair(List<Card> checkCards) {
                List<Card>? pairs = null;
                for (int i = 0; i < checkCards.Count - 1; i++) {
                    if (checkCards[i].rank == checkCards[i + 1].rank) {
                        if (pairs == null) {
                            pairs = new List<Card>() { checkCards[i], checkCards[i + 1] };
                            i++;
                        }
                        else {
                            pairs.Add(checkCards[i]);
                            pairs.Add(checkCards[i + 1]);
                            List<Card>? kickers = new List<Card>(checkCards);
                            for (int j = 0; j < 4; j++) {
                                kickers.Remove(pairs[j]);
                            }
                            if (kickers.Count == 0) {
                                kickers = null;
                            }
                            else {
                                kickers = kickers.GetRange(0, 1);
                            }
                            return new Hand(Hands.TwoPair, pairs, kickers);
                        }
                    }
                }
                return null;
            }

            static Hand? getBestPair(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 1; i++) {
                    if (checkCards[i].rank == checkCards[i + 1].rank) {
                        List<Card> pair = new List<Card>() { checkCards[i], checkCards[i + 1] };
                        List<Card> kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers = kickers.GetRange(0, kickers.Count >= 3 ? 3 : kickers.Count - 1);
                        pair.AddRange(kickers);

                        return new Hand(Hands.OnePair, pair);
                    }
                }
                return null;
            }
            static Hand getBestHighCard(List<Card> checkCards) {
                List<Card> result = checkCards.GetRange(0, checkCards.Count >= 5 ? 5 : checkCards.Count - 1);
                return new Hand(Hands.HighCard, result);
            }
        }


        static void PlayGame() {
            // Implement the game logic here
            Console.WriteLine("Game is starting...");
            // Example: Deal cards, evaluate hands, determine winner, etc.

            List<Card> cards = new List<Card>() { new Card(Rank.Three, Suit.Spades), new Card(Rank.Five, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Seven, Suit.Spades), new Card(Rank.Six, Suit.Hearts) };
            //Hand testHand = new Hand(cards);
            //Console.WriteLine(string.Join(", ", testHand.GetCards()));
            Console.WriteLine(Rank.Two - 1);
        }
    }

}