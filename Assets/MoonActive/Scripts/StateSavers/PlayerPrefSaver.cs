using UnityEngine;
using MoonActive.Scripts;

public class PlayerPrefSaver : StateSaver
{
    private const string PlayerPrefsKey = "TicTacToeBoard";
    private const string PlayerKey = "TicTacToePlayer";
    private readonly int _rows;
    private readonly int _cols;

    public PlayerPrefSaver(int rows, int cols){
        _rows = rows;
        _cols = cols;
    }

    public override void SaveBoardState(TileState[,] board)
    {
        // Convert the 2D array to a string for storage
        string serializedBoard = SerializeBoard(board);

        // Save the serialized board state to PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKey, serializedBoard);
    }


    public override void SavePlayerState(PlayerType currentPlayer)
    {
        // Save the player state to PlayerPrefs
        PlayerPrefs.SetString(PlayerKey, currentPlayer.ToString());
    }

    public override TileState[,] LoadBoardState()
    {
        // Load the serialized board state from PlayerPrefs
        string serializedBoard = PlayerPrefs.GetString(PlayerPrefsKey, string.Empty);

        // If there's a saved state, deserialize and return it; otherwise, return a new board
        return string.IsNullOrEmpty(serializedBoard) ? new TileState[_rows, _cols] : DeserializeBoard(serializedBoard);
    }

    public override PlayerType LoadPlayerState()
    {
        // Load the player state from PlayerPrefs
        string player = PlayerPrefs.GetString(PlayerKey, string.Empty);

        return StringToPlayerType(player);
    }

    private PlayerType StringToPlayerType(string player)
    {
        if(player == PlayerType.PlayerO.ToString())
        {
            return PlayerType.PlayerO;
        }

        // Default of PlayerX, i assumed if there is no save, then load default mode.
        return PlayerType.PlayerX;
    }

    private string SerializeBoard(TileState[,] board)
    {
        // Serialize the board state to a string, you can use JSON or any format of your choice
        string serializedBoard = "";

        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                serializedBoard += (int)board[row, col] + ",";
            }
        }

        // Remove the trailing comma
        if (!string.IsNullOrEmpty(serializedBoard))
        {
            serializedBoard = serializedBoard.TrimEnd(',');
        }

        return serializedBoard;
    }

    private TileState[,] DeserializeBoard(string serializedBoard)
    {
        // Deserialize the board state from the string
        string[] tileValues = serializedBoard.Split(',');

        TileState[,] deserializedBoard = new TileState[_rows, _cols];

        int index = 0;
        for (int row = 0; row < deserializedBoard.GetLength(0); row++)
        {
            for (int col = 0; col < deserializedBoard.GetLength(1); col++)
            {
                int tileValue = int.Parse(tileValues[index]);
                deserializedBoard[row, col] = (TileState)tileValue;
                index++;
            }
        }

        return deserializedBoard;
    }
}
