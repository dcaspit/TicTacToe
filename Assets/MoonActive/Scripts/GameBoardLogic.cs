using MoonActive.Scripts;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using log4net.Core;

public class GameBoardLogic
{
    private readonly GameView gameView;
    private readonly UserActionEvents userActionEvents;

    private StateSaver saverInMemory;
    private StateSaver saverPlayerPref;
    private WinningLogic winningLogic;

    private PlayerType currentPlayer;
    private TileState[,] board;

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
        this.saverInMemory = new InMemorySaver(rows, cols);
        this.saverPlayerPref = new PlayerPrefSaver(rows, cols);
        this.winningLogic = new GameWinningLogic(rows, cols);
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
        userActionEvents.LoadStateClicked += LoadClicked;

        InitializeBoard();
        currentPlayer = PlayerType.PlayerX;
        gameView.StartGame(currentPlayer);
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

    private void LoadClicked(GameStateSource source)
    {
        Debug.Log("Load clicked");
        if(source == GameStateSource.InMemory)
        {
            RestartGameAndLoadBoard(saverInMemory);
        }
        else if(source == GameStateSource.PlayerPrefs) 
        {
            RestartGameAndLoadBoard(saverPlayerPref);
        }
    }

    private void RestartGameAndLoadBoard(StateSaver saver) {
        gameView.StartGame(saver.savedPlayer);
        saver.savedPlayer = currentPlayer;
        LoadBoard(saver.LoadBoardState());
    }

    private void LoadBoard(TileState[,] boardToLoad)
    {
        board = new TileState[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                TileState state = boardToLoad[row, col];
                if(state != TileState.Empty) 
                {
                    SetTileSign(GetPlayerTypeFrom(state), new BoardTilePosition(row, col));
                }
            }
        }
    }

    public PlayerType GetPlayerTypeFrom(TileState state) {
        if(state == TileState.PlayerX) return PlayerType.PlayerX;
        return PlayerType.PlayerO;
    }

    private void SaveClicked(GameStateSource source)
    {
        switch(source) 
        {
            case GameStateSource.InMemory:
                SaveBoard(saverInMemory);
            break;
            case GameStateSource.PlayerPrefs:
                SaveBoard(saverPlayerPref);
            break;
        }
    }

    private void SaveBoard(StateSaver saver) {
        saver.savedPlayer = currentPlayer;
        saver.SaveBoardState(board);
    }

    private void HandleTileClicked(BoardTilePosition boardTilePosition)
    {
        if (IsValidMove(boardTilePosition))
        {
            SetTileSign(currentPlayer, boardTilePosition);

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
       return winningLogic.CheckForWin(board);
    }

    private bool CheckForTie()
    {
       return winningLogic.CheckForTie(board);
    }

}
