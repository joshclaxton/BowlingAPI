using Bowling.Function.Logic;
using Xunit;

namespace Bowling.Tests
{
    public class BowlingScoreCalculatorTests
    {
        BowlingScoreCalculator calculator;
        public BowlingScoreCalculatorTests()
        {
            calculator = new BowlingScoreCalculator();
        }

        [Fact]
        public void Should_EquateBowlingScoreResults_When_Equal()
        {
            var result1 = new BowlingScoreResult { Score = 20, Error = "I'm an error" };
            var result2 = new BowlingScoreResult { Score = 20, Error = "I'm an error" };
            Assert.True(result1.Equals(result2));
        }

        [Fact]
        public void Should_NotEquateBowlingScoreResults_When_ScoreNotEqual()
        {
            var result1 = new BowlingScoreResult { Score = 20, Error = "I'm an error" };
            var result2 = new BowlingScoreResult { Score = 2000, Error = "I'm an error" };
            Assert.False(result1.Equals(result2));
        }

        [Fact]
        public void Should_NotEquateBowlingScoreResults_When_ErrorNotEqual()
        {
            var result1 = new BowlingScoreResult { Score = 20, Error = "I'm an error" };
            var result2 = new BowlingScoreResult { Score = 20, Error = "I'm a different error" };
            Assert.False(result1.Equals(result2));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_HaveCorrectErrorMessage_When_InputNullOrEmpty(string rollScores)
        {
            var expected = new BowlingScoreResult { Score = null, Error = "Bowling game must contain at least one roll" };
            Assert.StrictEqual(expected, calculator.CalculateScore(rollScores));
        }

        [Fact]
        public void Should_HaveCorrectErrorMessage_When_InvalidCharacterInInput()
        {
            var expected = new BowlingScoreResult { Score = null, Error = "Bowling game may contain only 0-9, X, or S" };
            Assert.StrictEqual(expected, calculator.CalculateScore("45-9*"));
        }

        [Theory]
        [InlineData("83")]
        [InlineData("x65")]
        [InlineData("XXXXXXXXX92")]
        [InlineData("82")]
        [InlineData("x64")]
        [InlineData("XXXXXXXXX91")]
        public void Should_HaveCorrectErrorMessage_When_FrameScoreGreaterThanOrEqualTo10(string rollScores)
        {
            var expected = new BowlingScoreResult { Score = null, Error = "Bowling game may not contain a frame with a score of 10 or more without it being a Strike (X) or a Spare (S)" };
            Assert.StrictEqual(expected, calculator.CalculateScore(rollScores));
        }

        [Theory]
        [InlineData("XXXXXXXXXXXXX")]
        [InlineData("1212121212121212121212")]
        [InlineData("XXXXXXXXXXXX4")]
        [InlineData("XXXXXXXXXX4S5")]
        [InlineData("XXXXXXXXX455")]
        [InlineData("XXXXXXXX45455")]
        [InlineData("XXXXXXXX9S6S53")]
        public void Should_HaveCorrectErrorMessage_When_TooManyRolls(string rollScores)
        {
            var expected = new BowlingScoreResult { Score = null, Error = "Bowling game may only contain 10 frames" };
            Assert.StrictEqual(expected, calculator.CalculateScore(rollScores));
        }

        [Theory]
        [InlineData("82S")]
        [InlineData("S")]
        [InlineData("XS")]
        [InlineData("XS45")]
        [InlineData("XXXXXXXXXS")]
        public void Should_HaveCorrectErrorMessage_When_SpareOnFirstRollOfFrame(string rollScores)
        {
            var expected = new BowlingScoreResult { Score = null, Error = "Bowling game may not contain a spare (S) on the first roll of a frame" };
            Assert.StrictEqual(expected, calculator.CalculateScore(rollScores));
        }


        [Theory]
        [InlineData("45454545454545454545", 90)] //full game
        [InlineData("XXXXXXXXXXXX", 300)] //full game of strikes
        [InlineData("XXXXXXXXXXX3", 293)] //full game of strikes with last open
        [InlineData("XXXXXXXXX3SX", 273)] //full games strikes followed by spare with last roll a strike
        [InlineData("XXXXXXXXX3S3", 266)] //full game strikes followed by spare with last roll open
        [InlineData("XXXXXXXXX34", 257)] //full game of strikes with last frame open
        [InlineData("XXXXXXXXX3S", 253)] //partial game of strikes with 10th frame incomplete with spare
        [InlineData("X3S", 20)]
        [InlineData("XXXXXXXXXX", 240)] //partial game of strikes with 10th frame incomplete with 1 strike
        [InlineData("XXXXXXXXXXX", 270)] //partial game of strikes with 10th frame incomplete with 2 strikes
        [InlineData("4S6S8S2S5S7S8S9S1S3S3", 152)] //full game of spares ending with third roll incomplete
        [InlineData("4S6S8S2S5S7S8S9S1S34", 146)] //full game of spares ending open
        [InlineData("4S6S8S2S5S7S8S9S1S3SX", 159)] //full game of spares ending in strike
        [InlineData("00000000000000000000", 0)] //full zero game
        [InlineData("4545", 18)] // partial game ending with complete frame
        [InlineData("45454", 18)] //partial game ending with incomplete frame
        [InlineData("7", 0)]
        [InlineData("XX", 0)]
        [InlineData("6S", 0)]
        [InlineData("XXX", 30)]
        [InlineData("XXXX", 60)]
        [InlineData("4S4", 14)]
        [InlineData("4S41", 19)]
        [InlineData("4SX", 20)]
        [InlineData("4SX2", 20)]
        [InlineData("4SX25", 44)]
        [InlineData("XXXXXXXX9S6S5", 260)]
        [InlineData("23XXXXXXXXX23",252)]
        [InlineData("23X4SXXXXXXX23",232)]
        public void Should_ScoreCorrectly_When_ValidInput(string rollScores, int expected)
        {
            var expectedResult = new BowlingScoreResult() { Score = expected, Error = null };
            Assert.StrictEqual(expectedResult, calculator.CalculateScore(rollScores));
        }
    }
}
