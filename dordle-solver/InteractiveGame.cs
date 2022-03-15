using System;
using System.Collections.Generic;
using System.Linq;

namespace dordle_solver
{
    internal class InteractiveGame
    {
        private readonly IList<string> _words;
        private readonly int _boardCount;
        private readonly int _maxGuesses;

        public InteractiveGame(
            IEnumerable<string> words,
            int boardCount,
            int maxGuesses)
        {
            _words = words.ToList();
            _boardCount = boardCount;
            _maxGuesses = maxGuesses;
        }

        public void PlayGame()
        {
            var options = new PossibleWords[_boardCount];
            var openBoards = _boardCount;
            for (var i = 0; i < _boardCount; i++)
                options[i] = new PossibleWords(_words);

            for (int i = 0; i < _maxGuesses; i++)
            {
                var maxOptions = -1;
                var bestGuess = -1;
                for (var x = 0; x < _boardCount; x++)
                {
                    if (options[x] == null)
                        continue;

                    var count = options[x].OptionCount();
                    if (count == 1)
                    {
                        bestGuess = x;
                        break;
                    }
                    if (count > maxOptions)
                    {
                        maxOptions = count;
                        bestGuess = x;
                    }
                }
                var currGuess = options[bestGuess].BestGuess();
                Console.WriteLine($"Please guess: {currGuess}");
                for (var x = 0; x < _boardCount; x++)
                {
                    if (options[x] == null)
                        continue;

                    var result = PromptResult(x);
                    if (result == "GGGGG")
                    {
                        options[x] = null;
                        openBoards--;
                    }
                    else
                    {
                        options[x].AddClue(currGuess, result);
                    }
                }
                if (openBoards == 0)
                {
                    Console.WriteLine("All boards are solved - congrats!");
                    return;
                }
            }
            Console.WriteLine("Looks like you ran out of guesses. My fault.");
        }

        private static string PromptResult(int boardNum)
        {
            while (true)
            {
                Console.Write($"Result on board #{boardNum + 1}? ");
                var result = Console.ReadLine().ToUpper();
                if (!PossibleWords.IsValidResult(result))
                    Console.WriteLine("Incorrect format. Results should be five letters long. G for green, Y for yellow, and X for gray.");
                else
                    return result;
            }
        }
    }
}