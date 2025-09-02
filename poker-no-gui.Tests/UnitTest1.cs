using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using PokerGame;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PokerGame.Tests {
    public class HandTests {

        private readonly ITestOutputHelper output;

        public HandTests(ITestOutputHelper output) {
            this.output = output;
        }

        public static IEnumerable<object[]> GetTestsForGetHand() {
            //Royal Flush with extra A
            List<Card> test1 = new List<Card>() {
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Jack, Suit.Spades),
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Ten, Suit.Spades),
                new Card(Rank.Ace, Suit.Hearts)
            };
            Hand exp1 = new Hand(Hands.RoyalFlush, new List<Card>() {
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Queen, Suit.Spades),
                new Card(Rank.Jack, Suit.Spades),
                new Card(Rank.Ten, Suit.Spades)
            });
            yield return new object[] { exp1, test1};

            //Four of a Kind AAAA with extra K and 2
            List<Card> test2 = new List<Card>() { 
                new Card(Rank.Ace, Suit.Spades), 
                new Card(Rank.Ace, Suit.Hearts), 
                new Card(Rank.Ace, Suit.Diamonds), 
                new Card(Rank.Ace, Suit.Clubs), 
                new Card(Rank.King, Suit.Spades), 
                new Card(Rank.Two, Suit.Hearts) 
            };
            Hand exp2 = new Hand(Hands.FourOfAKind, new List<Card>() {
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Ace, Suit.Diamonds),
                new Card(Rank.Ace, Suit.Clubs),
                new Card(Rank.King, Suit.Spades)
            });
            yield return new object[] { exp2, test2 };

            //Four of a Kind 2222 with extra A and K
            List<Card> test3 = new List<Card>() {
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Two, Suit.Diamonds),
                new Card(Rank.King, Suit.Clubs),
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Two, Suit.Hearts)
            };
            Hand exp3 = new Hand(Hands.FourOfAKind, new List<Card>() {
                new Card(Rank.Two, Suit.Spades),
                new Card(Rank.Two, Suit.Hearts),
                new Card(Rank.Two, Suit.Diamonds),
                new Card(Rank.Two, Suit.Clubs),
                new Card(Rank.Ace, Suit.Spades)
            });

            yield return new object[] { exp3, test3};

            //Straight Flush 65432 Spades with extra suited A
            List<Card> test4 = new List<Card>() { 
                new Card(Rank.Four, Suit.Spades), 
                new Card(Rank.Three, Suit.Spades), 
                new Card(Rank.Ace, Suit.Diamonds), 
                new Card(Rank.Five, Suit.Spades), 
                new Card(Rank.Two, Suit.Spades), 
                new Card(Rank.Six, Suit.Spades) 
            };
            Hand exp4 = new Hand(Hands.StraightFlush, new List<Card>() {
                new Card(Rank.Six, Suit.Spades),
                new Card(Rank.Five, Suit.Spades),
                new Card(Rank.Four, Suit.Spades),
                new Card(Rank.Three, Suit.Spades),
                new Card(Rank.Two, Suit.Spades)
            });
            yield return new object[] { exp4, test4};

            //Full House KKK33 with extra 3 and A
            List<Card> test5 = new List<Card>() { 
                new Card(Rank.King, Suit.Spades), 
                new Card(Rank.Three, Suit.Spades), 
                new Card(Rank.King, Suit.Clubs), 
                new Card(Rank.Ace, Suit.Spades),
                new Card(Rank.Three, Suit.Hearts), 
                new Card(Rank.King, Suit.Diamonds),
                new Card(Rank.Three, Suit.Diamonds)
            };
            Hand exp5 = new Hand(Hands.FullHouse, new List<Card>() {
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.King, Suit.Clubs),
                new Card(Rank.King, Suit.Diamonds),
                new Card(Rank.Three, Suit.Hearts),
                new Card(Rank.Three, Suit.Diamonds)
            });
            yield return new object[] { exp5, test5};

            //Flush AQT75 Hearts with extra suited 3
            List<Card> test6 = new List<Card>() { 
                new Card(Rank.Seven, Suit.Hearts), 
                new Card(Rank.Ten, Suit.Hearts), 
                new Card(Rank.Queen, Suit.Hearts), 
                new Card(Rank.Three, Suit.Hearts), 
                new Card(Rank.Ace, Suit.Hearts), 
                new Card(Rank.Five, Suit.Hearts) 
            };
            Hand exp6 = new Hand(Hands.Flush, new List<Card>() {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Seven, Suit.Hearts),
                new Card(Rank.Five, Suit.Hearts)
            });
            yield return new object[] { exp6, test6};

            //Straight AKQJT suited and extra A offsuit
            List<Card> test7 = new List<Card>() {
                new Card(Rank.Ace, Suit.Clubs),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.King, Suit.Hearts),
                new Card(Rank.Ace, Suit.Hearts)
            };
            Hand exp7 = new Hand(Hands.RoyalFlush, new List<Card>() {
                new Card(Rank.Ace, Suit.Hearts),
                new Card(Rank.King, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.Ten, Suit.Hearts),
            });
            yield return new object[] { exp7, test7};

            //Straight KQJT9 suited and extra 8
            List<Card> test8 = new List<Card>() {
                new Card(Rank.Eight, Suit.Hearts),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.King, Suit.Hearts),
                new Card(Rank.Nine, Suit.Hearts)
            };
            Hand exp8 = new Hand(Hands.StraightFlush, new List<Card>() {
                new Card(Rank.King, Suit.Hearts),
                new Card(Rank.Queen, Suit.Hearts),
                new Card(Rank.Jack, Suit.Hearts),
                new Card(Rank.Ten, Suit.Hearts),
                new Card(Rank.Nine, Suit.Hearts)
            });
            yield return new object[] { exp8, test8 };

            //Three of a Kind 777K3 with extra 2
            List<Card> test9 = new List<Card>() {
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Seven, Suit.Hearts),
                new Card(Rank.Three, Suit.Diamonds),
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.Two, Suit.Spades)
            };
            Hand exp9 = new Hand(Hands.ThreeOfAKind, new List<Card>() {
                new Card(Rank.Seven, Suit.Spades),
                new Card(Rank.Seven, Suit.Hearts),
                new Card(Rank.Seven, Suit.Clubs),
                new Card(Rank.King, Suit.Spades),
                new Card(Rank.Three, Suit.Diamonds)
            });
            yield return new object[] { exp9, test9};

            //Two Pair 6644T with extra 2
            List<Card> test10 = new List<Card>() {
                new Card(Rank.Ten, Suit.Spades),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Six, Suit.Diamonds),
                new Card(Rank.Six, Suit.Spades),
                new Card(Rank.Four, Suit.Clubs),
                new Card(Rank.Two, Suit.Spades)
            };
            Hand exp10 = new Hand(Hands.TwoPair, new List<Card>() {
                new Card(Rank.Six, Suit.Diamonds),
                new Card(Rank.Six, Suit.Spades),
                new Card(Rank.Four, Suit.Clubs),
                new Card(Rank.Four, Suit.Hearts),
                new Card(Rank.Ten, Suit.Spades)
            });
            yield return new object[] { exp10, test10};
        }

        [Theory]
        [MemberData(nameof(GetTestsForGetHand))]
        public void GetHand_Returns_Correct_Hand(Hand exp, List<Card> test) {
            //TODO: Separate each case into its own test for better isolation
            Hand testHand = Poker.GetHand(test);
            output.WriteLine("Expected:");
            output.WriteLine(exp.Name.ToString());
            foreach (Card c in exp.Cards) {
                output.WriteLine(c.ToString());
            }
            output.WriteLine("_____________");
            output.WriteLine("Actual:");
            output.WriteLine(testHand.Name.ToString());
            foreach (Card c in testHand.Cards) {
                output.WriteLine(c.ToString());
            }
            output.WriteLine("_____________");
            Assert.True(Hand.IsEqual(exp, Poker.GetHand(test)));
        }
    }
}