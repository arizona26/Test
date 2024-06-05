namespace Test.Model
{
    public class TeamSearch
    {
        public Response[] response { get; set; }
    }

    public class Response
    {
        public Team1 team { get; set; }
    }

    public class Team1
    {
        public string country { get; set; }
        public int founded { get; set; }
        public int id { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
        public bool national { get; set; }
    }
}
