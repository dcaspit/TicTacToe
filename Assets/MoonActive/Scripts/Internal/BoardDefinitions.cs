using System;

namespace MoonActive.Scripts
{
    public abstract class UserActionEvents
    {
        public abstract event Action StartGameClicked;
        public abstract event Action<BoardTilePosition> TileClicked;
        public abstract event Action<GameStateSource> SaveStateClicked;
        public abstract event Action<GameStateSource> LoadStateClicked;
    }

    public abstract class GameView
    {
        public abstract void StartGame(PlayerType player);
        public abstract void SetTileSign(PlayerType player, BoardTilePosition tilePosition);
        public abstract void ChangeTurn(PlayerType player);
        public abstract void GameTie();
        public abstract void GameWon(PlayerType player);
    }

    public struct BoardTilePosition
    {
        public int Row;
        public int Column;
    
        public BoardTilePosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public struct BoardGridSize
    {
        public int Rows;
        public int Columns;

        public BoardGridSize(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
    }

    public enum PlayerType
    {
        PlayerX,
        PlayerO
    }

    public enum GameStateSource
    {
        InMemory,
        PlayerPrefs
    }
}