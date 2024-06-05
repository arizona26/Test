namespace Test.Model
{

    public class NameSearch
    {
        public Api api { get; set; }
    }

    public class Api
    {
        public int results { get; set; }
        public Team[] teams { get; set; }
    }

    public class Team
    {
        public int team_id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string logo { get; set; }
        public string country { get; set; }
        public int? founded { get; set; }
        public string venue_name { get; set; }
        public string venue_surface { get; set; }
        public string venue_address { get; set; }
        public string venue_city { get; set; }
        public int? venue_capacity { get; set; }
    }

}
