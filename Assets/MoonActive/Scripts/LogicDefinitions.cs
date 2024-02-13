using System;

namespace MoonActive.Scripts
{
    public abstract class StateSaver {
        public PlayerType savedPlayer;
        public abstract void SaveBoardState(TileState[,] board);
        public abstract TileState[,] LoadBoardState();
    }

    public abstract class WinningLogic {
        public abstract bool CheckForWin(TileState[,] board);
        public abstract bool CheckForTie(TileState[,] board);
    }
}
