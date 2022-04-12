namespace dordle_solver
{
    internal class GuessEngine
    {
        public static string BestGuess(PossibleWords[] possibleWords)
        {
            var maxOptions = -1;
            var bestGuess = -1;

            for (var x = 0; x < possibleWords.Length; x++)
            {
                if (possibleWords[x] == null)
                    continue;

                var count = possibleWords[x].OptionCount();
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
            return possibleWords[bestGuess].BestGuess();
        }
    }
}