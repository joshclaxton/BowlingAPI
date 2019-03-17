using System;

namespace Bowling.Function.Logic
{
    public class BowlingScoreResult : IEquatable<BowlingScoreResult>
    {
        public int? Score { get; set; } = null;
        public string Error { get; set; }

        public BowlingScoreResult() { }
        public BowlingScoreResult(int score)
        {
            Score = score;
        }

        public BowlingScoreResult(string error)
        {
            Error = error;
        }

        public bool Equals(BowlingScoreResult other)
        {
            return this.Score == other.Score && this.Error == other.Error;
        }
    }
}
