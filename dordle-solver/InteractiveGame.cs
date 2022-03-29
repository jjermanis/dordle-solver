using System;
using System.Collections.Generic;
using System.Linq;

namespace dordle_solver
{
    internal class InteractiveGame
    {
        private readonly IList<string> _words;

        public InteractiveGame(
            IEnumerable<string> words)
        {
            _words = words.ToList();
        }

        public GameDesc PromptGame()
        {
            Console.WriteLine("Welcome to dordle-solver. This program will help you solve multiboard Wordle puzzles,");
            Console.WriteLine("like Dordle, Quordle, etc.");
            var boardCount = PromptBoardCount();
            var gameMap = GameDesc.GAME_MAP;

            if (gameMap.ContainsKey(boardCount))
            {
                var desc = gameMap[boardCount];
                Console.WriteLine($"I know that game. {desc.Name}. {desc.MaxMoves} moves allowed.");
                return desc;
            }
            else
            {
                // TODO - allow arbitrary games
                Console.WriteLine("I am not familiar with that game - sorry.");
                throw new NotImplementedException();
            }
        }

        public void PlayGame(GameDesc gameDesc)
        {
            var boardCount = gameDesc.BoardCount;
            var maxGuesses = gameDesc.MaxMoves;

            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the five colors from the result. G for green, Y for yellow, and X for gray.");
            Console.WriteLine("You will be prompted for each board separately. Once a board is solved, the program");
            Console.WriteLine("will stop asking about it.");

            var options = new PossibleWords[boardCount];
            var openBoards = boardCount;
            for (var i = 0; i < boardCount; i++)
                options[i] = new PossibleWords(_words);

            for (int i = 0; i < maxGuesses; i++)
            {
                var maxOptions = -1;
                var bestGuess = -1;
                for (var x = 0; x < boardCount; x++)
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
                for (var x = 0; x < boardCount; x++)
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

        private static int PromptBoardCount()
        {
            while (true)
            {
                Console.Write("How many different boards does your game have? ");
                var result = Console.ReadLine();
                if (int.TryParse(result, out int count) && count > 0)
                    return count;
                Console.WriteLine("I'm expecting a positive number, like 2, 4, 8, etc.");
            }
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