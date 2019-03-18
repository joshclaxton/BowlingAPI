using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bowling.Function.Logic
{
    public class BowlingScoreCalculator
    {
        const int charIntDif = 48;
        const char strike = 'X';
        const char spare = 'S';
        private static readonly ScoringSectionResult spareOnFirstRollOfFrameResult = new ScoringSectionResult($"Bowling game may not contain a spare ({spare}) on the first roll of a frame");

        /// <summary>
        /// Calculate the score of a bowling game
        /// </summary>
        /// <param name="rollScores">String representing the individual rolls for a bowling game</param>
        /// <returns>Score with a value indicating success or error with a value indicating failure</returns>
        public BowlingScoreResult CalculateScore(string rollScores)
        {
            if (string.IsNullOrEmpty(rollScores))
            {
                return new BowlingScoreResult("Bowling game must contain at least one roll");
            }

            rollScores = rollScores.Trim().ToUpperInvariant();
            if (!Regex.IsMatch(rollScores, $"^[0-9{strike}{spare}]*$"))
            {
                return new BowlingScoreResult($"Bowling game may contain only 0-9, {strike}, or {spare}");
            }

            var scoringSectionsResult = GetScoringSections(rollScores);
            if (scoringSectionsResult.Error != null)
            {
                return new BowlingScoreResult(scoringSectionsResult.Error);
            }

            return CalculateScoreFromScoringSections(scoringSectionsResult.ScoringSections);
        }

        private ScoringSectionResult GetScoringSections(string rollScores)
        {
            var scoringSections = new List<string>();
            var isNewFrame = true;
            bool endScoring = false;
            for (var i = 0; i < rollScores.Length && !endScoring; i++)
            {
                char currentRollScore = rollScores[i];
                switch (currentRollScore)
                {
                    case strike:
                        var cannotScoreStrike = i + 2 > rollScores.Length - 1;
                        if (cannotScoreStrike)
                        {
                            var hasSpareOnFirstRollOfFrame = i + 1 <= rollScores.Length - 1 && rollScores[i + 1] == spare;
                            if (hasSpareOnFirstRollOfFrame)
                            {
                                return spareOnFirstRollOfFrameResult;
                            }
                            endScoring = true;
                            break;
                        }
                        scoringSections.Add(rollScores.Substring(i, 3));
                        isNewFrame = true;
                        break;
                    case spare:
                        if (isNewFrame)
                        {
                            return spareOnFirstRollOfFrameResult;
                        }
                        bool cannotScoreSpare = i + 1 > rollScores.Length - 1;
                        if (cannotScoreSpare)
                        {
                            endScoring = true;
                            break;
                        }
                        scoringSections.Add(rollScores.Substring(i, 2));
                        isNewFrame = true;
                        break;
                    case var val when val >= '0' && val <= '9':
                        bool canScoreOpenFrame = isNewFrame && i + 1 <= rollScores.Length - 1 && rollScores[i + 1] != spare;
                        if (canScoreOpenFrame)
                        {
                            scoringSections.Add(rollScores.Substring(i, 2));
                        }
                        isNewFrame = !isNewFrame;
                        break;
                    default:
                        throw new ArgumentException($"{currentRollScore} is invalid in {nameof(rollScores)}");
                }

                bool hasTooManyRolls = scoringSections.Count == 10 && i + scoringSections[9].Length - 1 < rollScores.Length - 1;
                if (hasTooManyRolls)
                {
                    return new ScoringSectionResult("Bowling game may only contain 10 frames");
                }
                if(scoringSections.Count == 10)
                {
                    endScoring = true;
                }
            }
            return new ScoringSectionResult(scoringSections);
        }

        private BowlingScoreResult CalculateScoreFromScoringSections(List<string> scoringSections)
        {
            var score = 0;
            foreach (var scoringSection in scoringSections)
            {
                char firstSymbol = scoringSection.First();
                switch (firstSymbol)
                {
                    case strike:
                        score += CalculateStrikeFramePoints(scoringSection);
                        break;
                    case spare:
                        score += CalculateSpareFramePoints(scoringSection);
                        break;
                    case var val when val >= '0' && val <= '9':
                        var scoringSectionScore = CalculateOpenFramePoints(scoringSection);
                        if (scoringSectionScore >= 10)
                        {
                            return new BowlingScoreResult($"Bowling game may not contain a frame with a score of 10 or more without it being a Strike ({strike}) or a Spare ({spare})");
                        }
                        score += scoringSectionScore;
                        break;
                    default:
                        throw new ArgumentException($"{firstSymbol} is invalid in {nameof(scoringSections)}");
                }
            }
            return new BowlingScoreResult(score);
        }

        private int CalculateOpenFramePoints(string rollScores)
        {
            return ConvertRollSymbolToPoints(rollScores[1]) + ConvertRollSymbolToPoints(rollScores[0]);
        }

        private int CalculateSpareFramePoints(string rollScores)
        {
            return 10 + ConvertRollSymbolToPoints(rollScores[1]);
        }

        private int CalculateStrikeFramePoints(string rollScores)
        {
            return ConvertRollSymbolToPoints(rollScores[0]) +
                (rollScores[2] == spare ?
                    10
                    : (ConvertRollSymbolToPoints(rollScores[1]) + ConvertRollSymbolToPoints(rollScores[2]))
                );
        }

        private int ConvertRollSymbolToPoints(char symbol)
        {
            switch (symbol)
            {
                case strike:
                    return 10;
                case var val when val >= '0' && val <= '9':
                    return val - charIntDif;
                default:
                    throw new ArgumentException($"Cannot convert {nameof(symbol)} ({symbol}) to point value");
            }
        }

        private class ScoringSectionResult
        {
            public List<string> ScoringSections { get; set; }
            public string Error { get; set; } = null;
            public ScoringSectionResult(List<string> scoringSections)
            {
                ScoringSections = scoringSections;
            }
            public ScoringSectionResult(string error)
            {
                Error = error;
            }
        }
    }
}
