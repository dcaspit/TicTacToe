using MoonActive.Scripts;

class GameStateSaver {

    private StateSaver saverInMemory;
    private StateSaver saverPlayerPref;

    public GameStateSaver(int rows, int cols)
    {
        this.saverInMemory = new InMemorySaver(rows, cols);
        this.saverPlayerPref = new PlayerPrefSaver(rows, cols);
    } 

    public StateSaver getSaverBy(GameStateSource source) {
        switch(source) 
        {
            case GameStateSource.PlayerPrefs:
                return saverPlayerPref;
            default:
                return saverInMemory;
        }
    }
 
}