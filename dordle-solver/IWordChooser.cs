namespace dordle_solver
{
    public interface IWordChooser
    {
        string BestGuess();

        void UpdateAfterGuess(string guess, string result);

        int OptionCount();
    }
}
