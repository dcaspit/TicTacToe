using MoonActive.Scripts;

public abstract class StateSaver {
    public abstract void SaveBoardState(TileState[,] board);
    public abstract void SavePlayerState(PlayerType currentPlayer);
    public abstract TileState[,] LoadBoardState();
    public abstract PlayerType LoadPlayerState();
}

public abstract class WinningLogic {
    public abstract bool CheckForWin(TileState[,] board, PlayerType player);
    public abstract bool CheckForTie(TileState[,] board);
}

public enum TileState
{
    Empty,
    PlayerX,
    PlayerO
}
