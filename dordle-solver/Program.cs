using System;
using System.IO;
using System.Linq;

namespace dordle_solver
{
    internal class Program
    {
        private const string INTERACTIVE_ARG = "-p";
        private const string TEST_ARG = "-t";

        private const string FILE_PATH = @"..\..\..\words.txt";

        private static void Main(string[] args)
        {
            var words = File.ReadLines(FILE_PATH).ToList();

            var arg = args.Length > 0 ? args[0] : INTERACTIVE_ARG;

            switch (arg)
            {
                case TEST_ARG:
                    // TODO - implement
                    throw new NotImplementedException();

                case INTERACTIVE_ARG:
                default:
                    var game = new InteractiveGame(words);
                    var gameDesc = game.PromptGame();
                    game.PlayGame(gameDesc);
                    break;
            }
        }
    }
}