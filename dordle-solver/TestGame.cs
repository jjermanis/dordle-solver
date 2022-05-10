using System;
using System.Collections.Generic;
using System.Linq;

namespace dordle_solver
{
    internal class TestGame
    {
        //private readonly bool SHOW_WORD_DETAILS = true;

        private readonly IList<string> _allWords;
        private readonly LetterDistribution _startingLetterDistribution;
        private readonly IList<string> _startingWordOptions;

        public TestGame(IEnumerable<string> words)
        {
            _allWords = words.ToList();
            _startingLetterDistribution = new LetterDistribution(words);
            _startingWordOptions = _allWords.ToList();
        }

        public void RunTest()
        {
            RunTestCase(1000, 2);
            RunTestCase(500, 4);
            RunTestCase(250, 8);
        }

        private void RunTestCase(int testSize, int boardCount)
        {
            var guessCount = GameDesc.GAME_MAP[boardCount].MaxMoves;

            int start = Environment.TickCount;
            Console.WriteLine($"Running test: {testSize} games, {boardCount} boards");
            var results = new ResultDistribution(guessCount);

            for (int i = 0; i < testSize; i++)
            {
                var words = new List<string>(boardCount);
                for (int b = 0; b < boardCount; b++)
                    words.Add(_allWords[i + b * testSize]);

                var score = PlayGame(words, boardCount, guessCount);
                if (score.HasValue)
                    results.ScoreCount[score.Value]++;
                else
                    results.Misses++;
            }

            Console.Write(results);
            Console.WriteLine($"Time: {Environment.TickCount - start} ms");
        }

        private IWordChooser CreateWordChooser(int guessCount)
            // => new PossibleWords(_allWords);
            => new MinimizeExpectedRemainingCasesChooser(_allWords, guessCount, false);

        public int? PlayGame(
            IList<string> words,
            int boardCount,
            int guessCount)
        {
            var openBoardCount = words.Count;
            var options = new IWordChooser[boardCount];
            for (var i = 0; i < boardCount; i++)
                options[i] = CreateWordChooser(guessCount);

            for (int i = 0; i < guessCount; i++)
            {
                var currGuess = GuessEngine.BestGuess(options);
                for (var b = 0; b < boardCount; b++)
                {
                    var result = GetResult(currGuess, words[b]);
                    if (result == "GGGGG")
                    {
                        openBoardCount--;
                        options[b] = null;
                    }
                    if (options[b] != null)
                        options[b].UpdateAfterGuess(currGuess, result);
                }
                if (openBoardCount == 0)
                    return i + 1;
            }
            return null;
        }

        private string GetResult(string guess, string target)
            => PossibleWords.CalcResult(guess, target);
    }
}