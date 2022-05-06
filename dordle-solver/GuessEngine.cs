namespace dordle_solver
{
    internal class GuessEngine
    {
        public static string BestGuess(IWordChooser[] wordChooser)
        {
            var maxOptions = -1;
            var bestGuess = -1;

            for (var x = 0; x < wordChooser.Length; x++)
            {
                if (wordChooser[x] == null)
                    continue;

                var count = wordChooser[x].OptionCount();
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
            return wordChooser[bestGuess].BestGuess();
        }
    }
}