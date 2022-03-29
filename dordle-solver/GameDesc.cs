using System.Collections.Generic;

namespace dordle_solver
{
    internal class GameDesc
    {
        internal static readonly IDictionary<int, GameDesc> GAME_MAP
            = new Dictionary<int, GameDesc>
        {
            { 1, new GameDesc { BoardCount = 1, MaxMoves = 6, Name = "Wordle, the original"} },
            { 2, new GameDesc { BoardCount = 2, MaxMoves = 7, Name = "Dordle"} },
            { 4, new GameDesc { BoardCount = 4, MaxMoves = 9, Name = "Quordle"} },
            { 8, new GameDesc { BoardCount = 8, MaxMoves = 13, Name = "Octordle"} },
        };

        public int BoardCount { get; set; }
        public int MaxMoves { get; set; }
        public string Name { get; set; }
    }
}