using Xunit;
using PokerGame;

namespace PokerGame.Tests {
    public class HandTests {
        public static IEnumerable<object[]> GetTestHands() {
            //Royal Flush with extra Ace
            yield return new object[] { Hands.RoyalFlush, new List<Card>() { new Card(Rank.Queen, Suit.Spades), new Card(Rank.Jack, Suit.Spades), new Card(Rank.Ace, Suit.Spades), new Card(Rank.King, Suit.Spades), new Card(Rank.Ten, Suit.Spades), new Card(Rank.Ace, Suit.Hearts) } };

            //Straight AKQJT offsuit and extra Ace
            yield return new object[] { Hands.Straight, new List<Card>() { new Card(Rank.Queen, Suit.Spades), new Card(Rank.Jack, Suit.Hearts), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.King, Suit.Spades), new Card(Rank.Ten, Suit.Hearts), new Card(Rank.Ace, Suit.Spades) } };

            //Straight Flush 23456 Spades with extra suited Ace
            yield return new object[] { Hands.StraightFlush, new List<Card>() { new Card(Rank.Four, Suit.Spades), new Card(Rank.Three, Suit.Spades), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Five, Suit.Spades), new Card(Rank.Two, Suit.Spades), new Card(Rank.Six, Suit.Spades) } };

            //Full House KKK33 with extra 3
            yield return new object[] { Hands.FullHouse, new List<Card>() { new Card(Rank.King, Suit.Spades), new Card(Rank.King, Suit.Clubs), new Card(Rank.King, Suit.Diamonds), new Card(Rank.Three, Suit.Hearts), new Card(Rank.Three, Suit.Diamonds), new Card(Rank.Three, Suit.Spades), new Card(Rank.Ace, Suit.Spades) } };

            //Two Pair 6644A
            yield return new object[] { Hands.TwoPair, new List<Card>() { new Card(Rank.Four, Suit.Spades), new Card(Rank.Six, Suit.Clubs), new Card(Rank.Ace, Suit.Diamonds), new Card(Rank.Four, Suit.Hearts), new Card(Rank.Two, Suit.Diamonds), new Card(Rank.Six, Suit.Spades) } };
        }

        [Theory]
        [MemberData(nameof(GetTestHands))]
        public void GetHand_Returns_Correct_Hand(Hands expected, List<Card> cards) {
            Hand result = Poker.GetHand(cards);
            Assert.Equal(expected, result.Name);
        }
    }
}