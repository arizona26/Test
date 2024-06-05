namespace Test.Model
{
    public class MatchSearch
    {
        public Api1 api { get; set; }
    }

    public class Api1
    {
        public int results { get; set; }
        public Fixture[] fixtures { get; set; }
    }

    public class Fixture
    {
        public int fixture_id { get; set; }
        public int league_id { get; set; }
        public League league { get; set; }
        public DateTime event_date { get; set; }
        public int event_timestamp { get; set; }
        public object firstHalfStart { get; set; }
        public object secondHalfStart { get; set; }
        public string round { get; set; }
        public string status { get; set; }
        public string statusShort { get; set; }
        public int elapsed { get; set; }
        public string venue { get; set; }
        public object referee { get; set; }
        public Hometeam homeTeam { get; set; }
        public Awayteam awayTeam { get; set; }
        public object goalsHomeTeam { get; set; }
        public object goalsAwayTeam { get; set; }
        public Score score { get; set; }
    }

    public class League
    {
        public string name { get; set; }
        public string country { get; set; }
        public string logo { get; set; }
        public object flag { get; set; }
    }

    public class Hometeam
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public string logo { get; set; }
    }

    public class Awayteam
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public string logo { get; set; }
    }

    public class Score
    {
        public object halftime { get; set; }
        public object fulltime { get; set; }
        public object extratime { get; set; }
        public object penalty { get; set; }
    }
}
