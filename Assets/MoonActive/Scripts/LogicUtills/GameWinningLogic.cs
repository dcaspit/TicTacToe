using MoonActive.Scripts;

class GameWinningLogic : WinningLogic
{

    public override bool CheckForTie(TileState[,] board)
    {
        int cols = board.GetLength(1);
        int rows = board.GetLength(0);
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

    public override bool CheckForWin(TileState[,] board, PlayerType player)
    {
        TileState currentPlayer = player == PlayerType.PlayerX ? TileState.PlayerX : TileState.PlayerO;
        return CheckRows(board, currentPlayer) || CheckCols(board, currentPlayer) || CheckDiagonaly(board, currentPlayer);
    }

    private bool CheckRows(TileState[,] board, TileState currentPlayer)
    {
        int counter;
        int cols = board.GetLength(1);
        int rows = board.GetLength(0);
        // Check if at least one row filled with currentPlayer
        for (int row = 0; row < rows; row++)
        {
            counter = 0;
            for(int col = 0; col < cols; col++)
            {
                // if at least one tile is empty, continue to next row
                if (board[row, col] == TileState.Empty) break; 

                if (board[row, col] == currentPlayer) counter++;

                if (counter == rows) return true; // if the row full of currentPlayer its a win!
            }
        }
        return false;
    }

    private bool CheckCols(TileState[,] board, TileState currentPlayer)
    {
        int counter;
        int cols = board.GetLength(1);
        int rows = board.GetLength(0);

        for (int col = 0; col < cols; col++)
        {
            counter = 0;
            for (int row = 0; row < rows; row++)
            {
                if (board[row, col] == TileState.Empty) break;

                if (board[row, col] == currentPlayer) counter++;

                if (counter == cols) return true;
            }
        }
        return false;
    }


    private bool CheckDiagonaly(TileState[,] board, TileState currentPlayer)
    {
        int counterForward = 0;
        int counterBackward = 0;
        int length = board.GetLength(0);
        for (int row = 0; row < length; row++)
        {
            if (board[row, row] == currentPlayer) counterForward++;
            if (board[length - row - 1, row] == currentPlayer) counterBackward++;
        }
        return (counterBackward == length) || (counterForward == length);
    }
}