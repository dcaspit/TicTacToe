using MoonActive.Scripts;
using UnityEngine;

public class GameBoardLogic
{
    private readonly GameView gameView;
    private readonly UserActionEvents userActionEvents;
    private GameStateSaver gameStateSaver;
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
        this.gameStateSaver = new GameStateSaver(rows, cols);
        this.winningLogic = new GameWinningLogic(rows, cols);
        userActionEvents.StartGameClicked += HandleStartGameClicked;
    }

    public void DeInitialize()
    {
        this.rows = -1;
        this.cols = -1;
        this.gameStateSaver = null;
        this.winningLogic = null;
        this.board = null;
        removeAllListeners();
    }

    private void removeAllListeners() 
    {
        userActionEvents.StartGameClicked -= HandleStartGameClicked;
        removeGameListeners();
    }

    private void removeGameListeners()
    {
        userActionEvents.TileClicked -= HandleTileClicked;
        userActionEvents.SaveStateClicked -= SaveClicked;
        userActionEvents.LoadStateClicked -= LoadClicked;
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
        RestartGameAndLoadBoard(gameStateSaver.getSaverBy(source));
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
        SaveBoard(gameStateSaver.getSaverBy(source));
    }

    private void SaveBoard(StateSaver saver) {
        saver.savedPlayer = currentPlayer;
        saver.SaveBoardState(board);
    }

    private void HandleTileClicked(BoardTilePosition boardTilePosition)
    {
        if (NotValidMove(boardTilePosition)) return;
        
        SetTileSign(currentPlayer, boardTilePosition);

        if (CheckForWin())
        {
            gameView.GameWon(currentPlayer);
            removeGameListeners();
        }
        else if (CheckForTie())
        {
            gameView.GameTie();
            removeGameListeners();
        }
        else
        {
            ChangeTurn();
        }
    }

    private bool NotValidMove(BoardTilePosition tilePosition)
    {
        return board[tilePosition.Row, tilePosition.Column] != TileState.Empty;
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
