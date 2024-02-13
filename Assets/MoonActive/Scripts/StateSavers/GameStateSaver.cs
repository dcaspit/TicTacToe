using MoonActive.Scripts;

class GameStateSaver {

    private readonly StateSaver _saverInMemory;
    private readonly StateSaver _saverPlayerPref;

    public GameStateSaver(int rows, int cols)
    {
        _saverInMemory = new InMemorySaver(rows, cols);
        _saverPlayerPref = new PlayerPrefSaver(rows, cols);
    } 

    public StateSaver getSaverBy(GameStateSource source) {
        return source switch
        {
            GameStateSource.PlayerPrefs => _saverPlayerPref,
            GameStateSource.InMemory => _saverInMemory,
            _ => throw new System.InvalidOperationException(),
        };
    }
 
}