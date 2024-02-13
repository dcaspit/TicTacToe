using MoonActive.Scripts;
using UnityEngine;

public class InMemorySaver : StateSaver
{

    private TileState[,] savedBoard;
    private int rows;
    private int cols;

    public InMemorySaver(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
    }

    public override TileState[,] LoadBoardState()
    {
        throw new System.NotImplementedException();
    }

    public override void SaveBoardState(TileState[,] board)
    {
        savedBoard = new TileState[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                savedBoard[row, col] = board[row, col];
            }
        }
    }
}