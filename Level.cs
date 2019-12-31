// This class defines the look of our json that we store in the database
public class Level
{
    public string DatabaseKey { get; set; } //this prop holds the key set by Firebase when the level is POSTed (unique key by Push) -- useable for example when trying to UPDATE specific levels
    public readonly string levelName;
    public readonly int[] someArrayData;
    public readonly string version;
    public readonly LevelStatistics statistics;

    public Level(string levelName, int[] someArrayData, string version, int deaths, int clears)
    {
        this.levelName = levelName;
        this.someArrayData = someArrayData;
        this.version = version;
        statistics = new LevelStatistics(deaths, clears);
    }

    //To give example for a bit more complex json
    public class LevelStatistics
    {
        public readonly int deaths;
        public readonly int clears;

        public LevelStatistics(int deaths, int clears)
        {
            this.deaths = deaths;
            this.clears = clears;
        }
    }
}