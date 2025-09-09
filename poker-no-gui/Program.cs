using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Transactions;


//TODO: Add documentation

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
        public Rank Rank;
        public Suit Suit;

        public Card(Rank rank, Suit suit) {
            this.Rank = rank;
            this.Suit = suit;
        }

        public override string ToString() {
            return $"{Rank} of {Suit}";
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

        public Hand(Hands name, List<Card> cards) {
            this.Name = name;
            this.Cards = new List<Card>(cards);
        }

        //Equality check for two hands
        public static bool IsEqual(Hand? first, Hand? second) {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) {
                return true;
            }
            if (ReferenceEquals(first, null) || ReferenceEquals(second, null)) {
                return false;
            }
            if (ReferenceEquals(first, second))
                return true;
            if (first.Name != second.Name)
                return false;
            if (first.Cards.Count != second.Cards.Count)
                return false;

            return CompareHands(first, second) == 0;
        }

        //Compares Two Best 5-Card (or less, if applicable) Hands (already gone through GetHand),
        //Returns negative if a is loses to b, 0 if a ties with b, positive if a beats b
        public static int CompareHands (Hand a, Hand b) {
            int comp = a.Name - b.Name;
            if (comp != 0)
                return comp;
            else {
                switch (a.Name) {
                    case Hands.RoyalFlush:
                        return 0;
                    case Hands.StraightFlush:
                    case Hands.Straight:
                        return a.Cards[0].Rank - b.Cards[0].Rank;
                    case Hands.Flush: {
                        for (int i = 0; i < a.Cards.Count; i++) {
                            comp = a.Cards[i].Rank - b.Cards[i].Rank;
                            if (comp != 0)
                                return comp;
                            else
                                continue;
                        }
                        return 0;
                    }
                    case Hands.FourOfAKind: {
                        comp = a.Cards[0].Rank - b.Cards[0].Rank;
                        if (comp != 0)
                            return comp;
                        else {
                            if (a.Cards.Count == 5)
                                return a.Cards[4].Rank - b.Cards[4].Rank;
                            else
                                return 0;
                        }
                    }
                    case Hands.FullHouse: {
                        comp = a.Cards[0].Rank - b.Cards[0].Rank;
                        if (comp != 0)
                            return comp;
                        else
                            return a.Cards[3].Rank - b.Cards[3].Rank;
                    }
                    case Hands.ThreeOfAKind: {
                        comp = a.Cards[0].Rank - b.Cards[0].Rank;
                        if (comp != 0)
                            return comp;
                        else {
                            if (a.Cards.Count > 3) {
                                for (int i = 3; i < a.Cards.Count; i++) {
                                    comp = a.Cards[i].Rank - b.Cards[i].Rank;
                                    if (comp != 0)
                                        return comp;
                                }
                                return 0;
                            }     
                            else
                                return 0;
                        }
                    }
                    case Hands.TwoPair: {
                        comp = a.Cards[0].Rank - b.Cards[0].Rank;
                        if (comp != 0)
                            return comp;
                        else {
                            comp = a.Cards[2].Rank - b.Cards[2].Rank;
                            if (comp != 0)
                                return comp;
                            else {
                                if (a.Cards.Count == 5)
                                    return a.Cards[4].Rank - b.Cards[4].Rank;
                                else
                                    return 0;
                            }
                        }
                    }
                    case Hands.OnePair: {
                        comp = a.Cards[0].Rank - b.Cards[0].Rank;
                        if (comp != 0)
                            return comp;
                        else {
                            if (a.Cards.Count > 2) {
                                for (int i = 2; i < a.Cards.Count; i++) {
                                    comp = a.Cards[i].Rank - b.Cards[i].Rank;
                                    if (comp != 0)
                                        return comp;
                                }
                                return 0;
                            }
                            else
                                return 0;
                        }
                    }
                    case Hands.HighCard: {
                        for (int i = 0; i < a.Cards.Count; i++) {
                            comp = a.Cards[i].Rank - b.Cards[i].Rank;
                            if (comp != 0)
                                return comp;
                        }
                        return 0;
                    }
                    default:
                        throw new ArgumentException("Hand(s) is (are) invalid");
                }
            }
        }

        public override string ToString() {
            string specName = "";
            switch (Name) {
                case Hands.RoyalFlush:
                    specName = "Royal Flush";
                    break;
                case Hands.StraightFlush:
                    specName = $"Straight Flush, {Cards[0].Rank} High";
                    break;
                case Hands.FourOfAKind:
                    specName = $"Four of a Kind, {Cards[0].Rank}s";
                    break;
                case Hands.FullHouse:
                    specName = $"Full House, {Cards[0].Rank}s full of {Cards[4].Rank}s";
                    break;
                case Hands.ThreeOfAKind:
                    specName = $"Three of a Kind, {Cards[0].Rank}s";
                    break;
                case Hands.Straight:
                case Hands.Flush:
                    specName = $"{Name}, {Cards[0].Rank} High";
                    break;
                case Hands.TwoPair:
                    specName = $"Two Pair, {Cards[0].Rank}s and {Cards[2].Rank}s";
                    break;
                case Hands.OnePair:
                    specName = $"Pair, {Cards[0].Rank}s";
                    break;
                case Hands.HighCard:
                    specName = $"High Card, {Cards[0].Rank} High";
                    break;
            }
            return $"Hand: {specName}\n{string.Join("\n", Cards)}";
        }
    }

    public class Player {
        //TODO: Add support for option to preselect turn action (e.g. check/fold, call any, fold, raise xx)

        public string Name { get; set; }
        public int ID { get; }

        //Folded, All-in, 
        public string State { get; set; }
        
        public List<Card> Cards { get; set; }

        public int Chips { get; }

        public bool Sitting { get; set; }

        //-1 for no action, 0 for check/fold, chip amount otherwise
        public int CurrentBet { get; set; }

        public Player(string Name, int ID, string State, int Chips = 10000, bool Sitting = true, int CurrentBet = -1) {
            this.Name = Name;
            this.ID = ID;
            this.State = State;
            this.Cards = new List<Card>();
            this.Chips = Chips;
            this.Sitting = Sitting;
            this.CurrentBet = CurrentBet;
        }
    }

    public class GameState {

        public int PrevPlayerID { get; }
        public string PrevAction { get; }

        public int PrevBet { get; }

        public GameState(int PrevPlayerID = -1, string PrevAction = "None", int PrevBet = -1) {
            this.PrevPlayerID = PrevPlayerID;
            this.PrevAction = PrevAction;
            this.PrevBet = PrevBet;
        }
    }

    public class Poker {

        static void Main(string[] args) {
            Console.WriteLine("Welcome to Akshit's Texas Hold 'Em Poker Game!");
            // Initialize game components here
            // For example, create a deck of cards, shuffle them, deal cards to players, etc.
            // Placeholder for game logic
            int numChips = -1;
            while (numChips == -1) {
                //TODO: Can change chip range based on game setup
                Console.WriteLine("Enter number of chips per player (between 500 and 10,000) or hit Enter to default to 10,000: ");
                string? numC = Console.ReadLine();
                if (numC == null)
                    numChips = 10000;
                if (numC == null || (Int32.TryParse(numC, out numChips) && numChips >= 500 && numChips <= 10000)) {
                    Console.WriteLine("Great! We'll begin with " + numChips + " chips!");
                }
                else {
                    Console.WriteLine("Sorry! That's not a valid number. Please try again!");
                }
            }

            int numPlayers = -1;
            while (numPlayers == -1) {
                Console.WriteLine("Enter number of players (between 2 and 10): ");
                string? numP = Console.ReadLine();
                if (numP != null && Int32.TryParse(numP, out numPlayers) && numPlayers >= 2 && numPlayers <= 10) {
                    Console.WriteLine("Great! We'll begin with " + numPlayers + " players!");
                }
                else {
                    Console.WriteLine("Sorry! That's not a valid number. Please try again!");
                }
            }

            List<Player> players = new List<Player>();
            while (players.Count < numPlayers) {
                Console.WriteLine("Please enter a name for Player " + (players.Count + 1) + ":");
                string? pName = Console.ReadLine();
                if (pName != null)
                    players.Add(new Player(pName, players.Count + 1, "idk yet", numChips));
                else
                    Console.WriteLine("Sorry! That name is invalid. Please try again!");
            }

            Console.WriteLine("Great! Initializing game components...");
            Deck playDeck = new Deck();
            List<List<Card>> playerCards = new List<List<Card>>();

            //TODO: Implement betting and dealing and all that jazz

            //Dealing out first two:
            bool second = false;
            for (int i = 0; i < players.Count; i++) {
                players[i].Cards.Add(playDeck.Pop());
                if (i == players.Count - 1 && !second) {
                    second = true;
                    i = 0;
                }
            }

            //Basic round loop
            StartRound(players);

        }

        public static Hand GetBestHand(List<Card> cards) {

            List<Card> sortedCards = new List<Card>(cards);
            sortedCards.Sort((a, b) => b.Rank - a.Rank);
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




            // Helper functions to evaluate different hands

            static Hand? getBestRoyalFlush(List<Card> checkCards) {
                Hand? bestRF = getBestStraightFlush(checkCards);
                if (bestRF != null && bestRF.Cards[0].Rank == Rank.Ace) {
                    bestRF.Name = Hands.RoyalFlush;
                    return bestRF;
                }
                return null;
            }

            static Hand? getBestStraightFlush(List<Card> checkCards) {
                List<Card> checkStraightFlush = new List<Card>(checkCards);
                checkStraightFlush.Sort((a, b) => a.Suit - b.Suit);
                for (int i = 0; i < checkStraightFlush.Count - 4; i++) {
                    List<Card> flush = new List<Card>() { checkStraightFlush[i] };
                    Suit currSuit = checkStraightFlush[i].Suit;
                    for (int j = i + 1; j < checkStraightFlush.Count; j++) {
                        if (checkStraightFlush[j].Suit == currSuit)
                            flush.Add(checkStraightFlush[j]);
                        else
                            break;
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
                    if (checkCards[i].Rank == checkCards[i + 1].Rank && checkCards[i].Rank == checkCards[i + 2].Rank && checkCards[i].Rank == checkCards[i + 3].Rank) {
                        List<Card> four = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2], checkCards[i + 3] };
                        List<Card> kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);
                        kickers.Remove(checkCards[i + 3]);
                        if (kickers.Count > 0)
                            four.Add(kickers[0]);
                        return new Hand(Hands.FourOfAKind, four);
                    }
                }
                return null;
            }

            static Hand? getBestFullHouse(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 2; i++) {
                    if (checkCards[i].Rank == checkCards[i + 1].Rank && checkCards[i].Rank == checkCards[i + 2].Rank) {
                        List<Card> full = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2] };
                        List<Card>? kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);

                        for (int j = 0; j < kickers.Count - 1; j++) {
                            if (kickers[j].Rank == kickers[j + 1].Rank) {
                                full.Add(kickers[i]);
                                full.Add(kickers[i + 1]);
                                return new Hand(Hands.FullHouse, full);
                            }
                        }
                    }
                }
                return null;
            }

            static Hand? getBestFlush(List<Card> checkCards) {
                List<Card> checkFlush = new List<Card>(checkCards);
                checkFlush.Sort((a, b) => a.Suit - b.Suit);
                for (int i = 0; i < checkFlush.Count - 4; i++) {
                    List<Card> flush = new List<Card>() { checkFlush[i] };
                    Suit currSuit = checkFlush[0].Suit;
                    for (int j = i + 1; j < i + 5; j++) {
                        if (checkFlush[j].Suit == currSuit)
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
                    if (checkCards[i].Rank == Rank.Ace)
                        checkStraight.Add(checkCards[i]);
                    else
                        break;
                }

                for (int i = 0; i < checkStraight.Count - 4; i++) {
                    List<Card> straight = new List<Card>() { checkStraight[i] };
                    Rank lastRank = checkStraight[i].Rank;
                    for (int j = i + 1; j < checkStraight.Count; j++) {
                        if (checkStraight[j].Rank == lastRank)
                            continue;
                        else if (checkStraight[j].Rank == lastRank - 1 || checkStraight[j].Rank == lastRank + 12) {
                            straight.Add(checkStraight[j]);
                            lastRank = checkStraight[j].Rank;
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
                    if (checkCards[i].Rank == checkCards[i + 1].Rank && checkCards[i].Rank == checkCards[i + 2].Rank) {
                        List<Card> three = new List<Card>() { checkCards[i], checkCards[i + 1], checkCards[i + 2] };
                        List<Card> kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers.Remove(checkCards[i + 2]);
                        if (kickers.Count > 0)
                            kickers = kickers.GetRange(0, kickers.Count >= 2 ? 2 : kickers.Count);
                        three.AddRange(kickers);
                        return new Hand(Hands.ThreeOfAKind, three);
                    }
                }
                return null;
            }

            static Hand? getBestTwoPair(List<Card> checkCards) {
                List<Card>? pairs = null;
                for (int i = 0; i < checkCards.Count - 1; i++) {
                    if (checkCards[i].Rank == checkCards[i + 1].Rank) {
                        if (pairs == null) {
                            pairs = new List<Card>() { checkCards[i], checkCards[i + 1] };
                            i++;
                        }
                        else {
                            pairs.Add(checkCards[i]);
                            pairs.Add(checkCards[i + 1]);
                            List<Card> kickers = new List<Card>(checkCards);
                            for (int j = 0; j < 4; j++) {
                                kickers.Remove(pairs[j]);
                            }
                            if (kickers.Count > 0)
                                pairs.Add(kickers[0]);
                            return new Hand(Hands.TwoPair, pairs);
                        }
                    }
                }
                return null;
            }

            static Hand? getBestPair(List<Card> checkCards) {
                for (int i = 0; i < checkCards.Count - 1; i++) {
                    if (checkCards[i].Rank == checkCards[i + 1].Rank) {
                        List<Card> pair = new List<Card>() { checkCards[i], checkCards[i + 1] };
                        List<Card> kickers = new List<Card>(checkCards);
                        kickers.Remove(checkCards[i]);
                        kickers.Remove(checkCards[i + 1]);
                        kickers = kickers.GetRange(0, kickers.Count >= 3 ? 3 : kickers.Count);
                        pair.AddRange(kickers);

                        return new Hand(Hands.OnePair, pair);
                    }
                }
                return null;
            }
            static Hand getBestHighCard(List<Card> checkCards) {
                List<Card> result = checkCards.GetRange(0, checkCards.Count >= 5 ? 5 : checkCards.Count);
                return new Hand(Hands.HighCard, result);
            }
        }

        //Returns list of player numbers with winning hands (players 1 to n)
        public static List<int> GetWinningHand(List<List<Card>> playerCards) {
            List<Hand> playerHands = new List<Hand>();
            int highest = -1;
            List<int> best = new List<int>();
            int currPlayer = 0;

            foreach (List<Card> player in playerCards) {
                currPlayer++;
                Hand currHand = GetBestHand(player);
                playerHands.Add(currHand);
                if ((int)currHand.Name > highest) {
                    highest = (int)currHand.Name;
                    best = new List<int>();
                    best.Add(currPlayer);
                }
                else if ((int)currHand.Name == highest)
                    best.Add(currPlayer);
                else
                    continue;
            }

            //Case of one winner
            if (best.Count == 1)
                return best;
            //Case of two or more players with the same type of hand
            else {
                Hand currBest = playerHands[best[0] - 1];
                List<int> newBest = new List<int>();
                newBest.Add(best[0]);
                for ( int i = 1; i < best.Count; i++ ) {
                    int comp = Hand.CompareHands(currBest, playerHands[best[i] - 1]);
                    if (comp < 0) {
                        currBest = playerHands[best[i] - 1];
                        newBest = new List<int>();
                        newBest.Add(best[i]);
                    }
                    else if (comp == 0) {
                        newBest.Add(best[i]);
                    }
                    else
                        continue;
                }
                return newBest;
            }
        }

        static void StartRound(List<Player> players) {
            // TODO: Implement the game logic for any single round here

            /*
             * 1) Reset current bet for all players (each at a time would mess up loop arounds)
             * 2) Player validation (hasn't folded, has chips, is turn, etc.)
             * 2) Evaluate current state (is there a previous bet vs check/first turn, are you little blind (second time around on first round), etc)
             * 3) Present player with options (previous check or first turn - check, raise, fold (confirmation message since dumb), previous bet not all in amount - call, raise, fold, previous bet all in amount - all in, fold)
             * 4) Process choices (check - next player; fold - set player to out, check if game over; all in - same conditions as in raise, make sure keep all in amount aside from following bets; raise - show options (custom, double previous (if applicable), half pot, pot, all in), confirm valid amount, add chips to pot, remove from player, change current round bet amount, make round loop back to just before this player);
             * 5) Set to next player (if next player hasn't gone yet, hasn't reacted to a bet, or is small blind on first round second time around); end round (all bets called or folded, someone has won, etc); or end game (river round is over, everyone is all in except one, everyone folded except one, etc)
             */

            int currPlayer = 1;
            
            void resetBets() {
                foreach (Player player in players)
                    player.CurrentBet = -1;
            }

            bool validatePlayer(Player player) {
                if (player.ID != currPlayer)
                    return false;
                else if (!player.Sitting)
                    return false;
                else if (player.State == "Folded" || player.State == "All in")
                    return false;
                else
                    return true;
            }

            string evaluateState() {
                //TODO: Make a global game state class or enum to give an evaluation and check the previous state/playeraction
                return "IDK";
            }

        }
    }

}