using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dordle_solver
{
    internal class Program
    {
        private static readonly IDictionary<int, GameDesc> GAME_MAP = new Dictionary<int, GameDesc>
        {
            { 1, new GameDesc { BoardCount = 1, MaxMoves = 6, Name = "Wordle, the original"} },
            { 2, new GameDesc { BoardCount = 2, MaxMoves = 7, Name = "Dordle"} },
            { 4, new GameDesc { BoardCount = 4, MaxMoves = 9, Name = "Quordle"} },
            { 8, new GameDesc { BoardCount = 8, MaxMoves = 13, Name = "Octordle"} },
        };

        private const string FILE_PATH = @"..\..\..\words.txt";

        private static void Main(string[] _)
        {
            var words = File.ReadLines(FILE_PATH).ToList();

            Console.WriteLine("Welcome to dordle-solver. This program will help you solve multiboard Wordle puzzles,");
            Console.WriteLine("like Dordle, Quordle, etc.");
            var boardCount = PromptBoardCount();

            int maxMoves;
            if (GAME_MAP.ContainsKey(boardCount))
            {
                var desc = GAME_MAP[boardCount];
                Console.WriteLine($"I know that game. {desc.Name}. {desc.MaxMoves} moves allowed.");
                maxMoves = desc.MaxMoves;
            }
            else
            {
                // TODO - allow arbitrary games
                Console.WriteLine("I am not familiar with that game - sorry.");
                return;
            }

            Console.WriteLine("To use, guess what this program suggests. Then, let this program know the result.");
            Console.WriteLine("Enter the five colors from the result. G for green, Y for yellow, and X for gray.");
            Console.WriteLine("You will be prompted for each board separately. Once a board is solved, the program");
            Console.WriteLine("will stop asking about it.");
            var game = new InteractiveGame(words, boardCount, maxMoves);
            game.PlayGame();
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
    }
}