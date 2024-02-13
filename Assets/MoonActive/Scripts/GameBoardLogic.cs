using MoonActive.Scripts;
using UnityEngine;

public class GameBoardLogic
{
    private readonly GameView _gameView;
    private readonly UserActionEvents _userActionEvents;
    private GameStateSaver _gameStateSaver;
    private WinningLogic _winningLogic;
    private PlayerType _currentPlayer;
    private TileState[,] _board;
    private int _rows;
    private int _cols;

    public GameBoardLogic(GameView gameView, UserActionEvents userActionEvents)
    {
        _gameView = gameView;
        _userActionEvents = userActionEvents;
    }

    public void Initialize(int columns, int rows)
    {
        _rows = rows;
        _cols = columns;
        _gameStateSaver = new GameStateSaver(_rows, _cols);
        _winningLogic = new GameWinningLogic();
        _userActionEvents.StartGameClicked += HandleStartGameClicked;
    }

    private void HandleStartGameClicked()
    {
        AddGameListeners();
        InitializeBoard();
        _currentPlayer = PlayerType.PlayerX;
        _gameView.StartGame(_currentPlayer);
    }

    private void AddGameListeners() 
    {
        _userActionEvents.TileClicked += HandleTileClicked;
        _userActionEvents.SaveStateClicked += HandleSaveClicked;
        _userActionEvents.LoadStateClicked += HandleLoadClicked;
    }

    private void InitializeBoard()
    {
        _board = new TileState[_rows, _cols];
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _board[row, col] = TileState.Empty;
            }
        }
    }

    private void HandleLoadClicked(GameStateSource source)
    {
        // Get State Saver logic implementation
        StateSaver stateSaver = _gameStateSaver.getSaverBy(source);

        // Restart Game with saved player
        _currentPlayer = stateSaver.LoadPlayerState();

        Debug.Log("Loaded: " + _currentPlayer.ToString());
        _gameView.StartGame(_currentPlayer);
        LoadBoard(stateSaver.LoadBoardState());
    }

    private void LoadBoard(TileState[,] boardToLoad)
    {
        // Reset the board 
        _board = new TileState[_rows, _cols];
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                TileState state = boardToLoad[row, col];
                if(state != TileState.Empty) 
                {
                    SetTileSign(GetPlayerTypeFrom(state), new BoardTilePosition(row, col));
                }
            }
        }
    }

    public PlayerType GetPlayerTypeFrom(TileState state) 
    {
        // Function assumes TileState is not empty;
        if(state == TileState.PlayerX) return PlayerType.PlayerX;
        return PlayerType.PlayerO;
    }

    private void HandleSaveClicked(GameStateSource source)
    {
        SaveBoard(_gameStateSaver.getSaverBy(source));
    }

    private void SaveBoard(StateSaver saver) 
    {
        Debug.Log("Saved: " + _currentPlayer.ToString());
        saver.SavePlayerState(_currentPlayer);
        saver.SaveBoardState(_board);
    }

    private void HandleTileClicked(BoardTilePosition boardTilePosition)
    {
        if (NotValidMove(boardTilePosition)) return;
        
        SetTileSign(_currentPlayer, boardTilePosition);

        if (CheckForWin())
        {
            _gameView.GameWon(_currentPlayer);
            RemoveGameListeners();
        }
        else if (CheckForTie())
        {
            _gameView.GameTie();
            RemoveGameListeners();
        }
        else
        {
            ChangeTurn();
        }
    }

    private bool NotValidMove(BoardTilePosition tilePosition)
    {
        return _board[tilePosition.Row, tilePosition.Column] != TileState.Empty;
    }

    private void SetTileSign(PlayerType player, BoardTilePosition tilePosition)
    {
        _board[tilePosition.Row, tilePosition.Column] = (player == PlayerType.PlayerX) ? TileState.PlayerX : TileState.PlayerO;
        _gameView.SetTileSign(player, tilePosition);
    }

    private void ChangeTurn()
    {
        _currentPlayer = (_currentPlayer == PlayerType.PlayerX) ? PlayerType.PlayerO : PlayerType.PlayerX;
        _gameView.ChangeTurn(_currentPlayer);
    }

    private bool CheckForWin()
    {
       return _winningLogic.CheckForWin(_board, _currentPlayer);
    }

    private bool CheckForTie()
    {
       return _winningLogic.CheckForTie(_board);
    }

    public void DeInitialize()
    {
        this._rows = -1;
        this._cols = -1;
        this._gameStateSaver = null;
        this._winningLogic = null;
        this._board = null;
        RemoveAllListeners();
    }

    private void RemoveAllListeners() 
    {
        _userActionEvents.StartGameClicked -= HandleStartGameClicked;
        RemoveGameListeners();
    }

    private void RemoveGameListeners()
    {
        _userActionEvents.TileClicked -= HandleTileClicked;
        _userActionEvents.SaveStateClicked -= HandleSaveClicked;
        _userActionEvents.LoadStateClicked -= HandleLoadClicked;
    }

}
