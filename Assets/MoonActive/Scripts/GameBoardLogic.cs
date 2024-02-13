using MoonActive.Scripts;
using UnityEngine;
using System.Collections;

public class GameBoardLogic
{
    private readonly GameView gameView;
    private readonly UserActionEvents userActionEvents;

    private PlayerType currentPlayer;
    private TileState[,] board;
    private TileState[,] inMemoryBoard;

    private int rows;
    private int cols;

    public GameBoardLogic(GameView gameView, UserActionEvents userActionEvents)
    {
        this.gameView = gameView;
        this.userActionEvents = userActionEvents;
    }

    public void Initialize(int columns, int rows)
    {
        this.rows = rows;
        this.cols = columns;
        userActionEvents.StartGameClicked += HandleStartGameClicked;
    }

    public void DeInitialize()
    {
        userActionEvents.StartGameClicked -= HandleStartGameClicked;
        userActionEvents.TileClicked -= HandleTileClicked;
    }

    private void HandleStartGameClicked()
    {
        userActionEvents.TileClicked += HandleTileClicked;
        userActionEvents.SaveStateClicked += SaveClicked;
        InitializeBoard();
        currentPlayer = PlayerType.PlayerX;
        gameView.StartGame(currentPlayer);
    }

    private void SaveClicked(GameStateSource source)
    {
        if(source == GameStateSource.InMemory)
        {
            inMemoryBoard = board;
        }
    }

    private void InitializeBoard()
    {
        board = new TileState[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                board[row, col] = TileState.Empty;
            }
        }
    }

    private void HandleTileClicked(BoardTilePosition boardTilePosition)
    {
        if (IsValidMove(boardTilePosition))
        {
            SetTileSign(currentPlayer, boardTilePosition);
            gameView.SetTileSign(currentPlayer, boardTilePosition);

            if (CheckForWin())
            {
                gameView.GameWon(currentPlayer);
            }
            else if (CheckForTie())
            {
                gameView.GameTie();
            }
            else
            {
                ChangeTurn();
            }
        }
    }

    private bool IsValidMove(BoardTilePosition tilePosition)
    {
        return board[tilePosition.Row, tilePosition.Column] == TileState.Empty;
    }

    private void SetTileSign(PlayerType player, BoardTilePosition tilePosition)
    {
        board[tilePosition.Row, tilePosition.Column] = (player == PlayerType.PlayerX) ? TileState.PlayerX : TileState.PlayerO;
        gameView.SetTileSign(player, tilePosition);
    }

    private void ChangeTurn()
    {
        currentPlayer = (currentPlayer == PlayerType.PlayerX) ? PlayerType.PlayerO : PlayerType.PlayerX;
        gameView.ChangeTurn(currentPlayer);
    }

    private bool CheckForWin()
    {
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

    private bool CheckForTie()
    {
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

}
