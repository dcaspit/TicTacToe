using MoonActive.Scripts;


class GameWinningLogic : WinningLogic
{
    private int rows;
    private int cols;
    private TileState[,] board;

    public GameWinningLogic(int rows, int cols) 
    {
        this.rows = rows;
        this.cols = cols;
    }

    public override bool CheckForTie(TileState[,] board)
    {
        this.board = board;
         // Check if any empty tile is found
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
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
        this.board = board;
        // Check rows, columns, and diagonals in a single loop
        for (int i = 0; i < rows; i++)
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
            (board[0, cols - 1] != TileState.Empty && AreTilesEqual(0, cols - 1, 1, 1) && AreTilesEqual(1, 1, 2, 0)))
        {
            return true;
        }

        return false;
    }

    private bool AreTilesEqual(int row1, int col1, int row2, int col2)
    {
        return board[row1, col1] == board[row2, col2];
    }
}