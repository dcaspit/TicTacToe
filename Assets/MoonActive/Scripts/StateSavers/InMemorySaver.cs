using MoonActive.Scripts;

public class InMemorySaver : StateSaver
{
    private TileState[,] _savedBoard;
    private PlayerType _savedPlayer;
    private readonly int _rows;
    private readonly int _cols;

    public InMemorySaver(int rows, int cols)
    {
        _rows = rows;
        _cols = cols;
        _savedBoard = new TileState[rows, cols];
        _savedPlayer = PlayerType.PlayerX;
    }

    public override TileState[,] LoadBoardState()
    {
        return _savedBoard;
    }

    public override void SaveBoardState(TileState[,] board)
    {
        _savedBoard = new TileState[_rows, _cols];
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _savedBoard[row, col] = board[row, col];
            }
        }
    }

    public override void SavePlayerState(PlayerType currentPlayer)
    {
        _savedPlayer = currentPlayer;
    }

    public override PlayerType LoadPlayerState()
    {
        return _savedPlayer;
    }
}