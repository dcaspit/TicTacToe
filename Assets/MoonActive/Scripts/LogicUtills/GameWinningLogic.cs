
class GameWinningLogic : WinningLogic
{
    private readonly int _rows;
    private readonly int _cols;
    private TileState[,] _board;

    public GameWinningLogic(int rows, int cols) 
    {
        _rows = rows;
        _cols = cols;
    }

    public override bool CheckForTie(TileState[,] board)
    {
        _board = board;
         // Check if any empty tile is found
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                if (board[row, col] == TileState.Empty)
                {
                    return false; // If any empty tile found, the game is not a tie
                }
            }
        }

        // If no empty tiles found, it's a tie
        return true;
    }

    public override bool CheckForWin(TileState[,] board)
    {
        this._board = board;
        // Check rows, columns, and diagonals in a single loop
        for (int i = 0; i < _rows; i++)
        {
            // Check rows and columns
            if ((board[i, 0] != TileState.Empty && AreTilesEqual(i, 0, i, 1) && AreTilesEqual(i, 1, i, 2)) ||
                (board[0, i] != TileState.Empty && AreTilesEqual(0, i, 1, i) && AreTilesEqual(1, i, 2, i)))
            {
                return true;
            }
        }

        // Check diagonals
        if ((board[0, 0] != TileState.Empty && AreTilesEqual(0, 0, 1, 1) && AreTilesEqual(1, 1, 2, 2)) ||
            (board[0, _cols - 1] != TileState.Empty && AreTilesEqual(0, _cols - 1, 1, 1) && AreTilesEqual(1, 1, 2, 0)))
        {
            return true;
        }

        return false;
    }

    private bool AreTilesEqual(int row1, int col1, int row2, int col2)
    {
        return _board[row1, col1] == _board[row2, col2];
    }
}