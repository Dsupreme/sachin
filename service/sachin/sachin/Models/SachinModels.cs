using System;
using System.Collections.Generic;


namespace sachin.Models
{
    public class FiltersModel
    {
        public dynamic selectedCountries { get; set; }
        public dynamic selectedInnings { get; set; }
        public dynamic selectedResult { get; set; }
        public dynamic selectedGround { get; set; }
        public FiltersModel()
        {
            selectedCountries = new Object();
            selectedInnings = new Object();
            selectedResult = new Object();
            selectedGround = new Object();
        }
    }

    public class GroundLocationModel
    {
        public Dictionary<string, List<string>> groundlocation { get; set; }
        public GroundLocationModel()
        {
            groundlocation = new Dictionary<string, List<string>>();
            groundlocation.Add("Home", new List<string>());
            groundlocation.Add("Away", new List<string>());

            // Segregating grounds on the basis of location
            groundlocation["Home"].Add("Nagpur");
            groundlocation["Home"].Add("Pune");
            groundlocation["Home"].Add("Margao");
            groundlocation["Home"].Add("Chandigarh");
            groundlocation["Home"].Add("Cuttack");
            groundlocation["Home"].Add("Kolkata");
            groundlocation["Home"].Add("Gwalior");
            groundlocation["Home"].Add("Dunedin");
            groundlocation["Home"].Add("New Delhi");
            groundlocation["Home"].Add("Jaipur");
            groundlocation["Home"].Add("Bangalore");
            groundlocation["Home"].Add("Jamshedpur");
            groundlocation["Home"].Add("Faridabad");
            groundlocation["Home"].Add("Guwahati");
            groundlocation["Home"].Add("Kanpur");
            groundlocation["Home"].Add("Ahmedabad");
            groundlocation["Home"].Add("Indore");
            groundlocation["Home"].Add("Mohali");
            groundlocation["Home"].Add("Rajkot");
            groundlocation["Home"].Add("Hyderabad (Deccan)");
            groundlocation["Home"].Add("Jalandhar");
            groundlocation["Home"].Add("Mumbai");
            groundlocation["Home"].Add("Chennai");
            groundlocation["Home"].Add("Vadodara");
            groundlocation["Home"].Add("Delhi");
            groundlocation["Home"].Add("Visakhapatnam");
            groundlocation["Home"].Add("Amritsar");
            groundlocation["Home"].Add("Mumbai (BS)");
            groundlocation["Home"].Add("Kochi");
            groundlocation["Home"].Add("Jodhpur");

            groundlocation["Away"].Add("Gujranwala");
            groundlocation["Away"].Add("Wellington");
            groundlocation["Away"].Add("Sharjah");
            groundlocation["Away"].Add("Leeds");
            groundlocation["Away"].Add("Nottingham");
            groundlocation["Away"].Add("Perth");
            groundlocation["Away"].Add("Hobart");
            groundlocation["Away"].Add("Adelaide");
            groundlocation["Away"].Add("Brisbane");
            groundlocation["Away"].Add("Sydney");
            groundlocation["Away"].Add("Melbourne");
            groundlocation["Away"].Add("Mackay");
            groundlocation["Away"].Add("Hamilton");
            groundlocation["Away"].Add("Harare");
            groundlocation["Away"].Add("Cape Town");
            groundlocation["Away"].Add("Port Elizabeth");
            groundlocation["Away"].Add("Centurion");
            groundlocation["Away"].Add("Johannesburg");
            groundlocation["Away"].Add("Bloemfontein");
            groundlocation["Away"].Add("Durban");
            groundlocation["Away"].Add("East London");
            groundlocation["Away"].Add("Colombo (RPS)");
            groundlocation["Away"].Add("Moratuwa");
            groundlocation["Away"].Add("Napier");
            groundlocation["Away"].Add("Auckland");
            groundlocation["Away"].Add("Christchurch");
            groundlocation["Away"].Add("Colombo (SSC)");
            groundlocation["Away"].Add("Singapore");
            groundlocation["Away"].Add("The Oval");
            groundlocation["Away"].Add("Manchester");
            groundlocation["Away"].Add("Toronto");
            groundlocation["Away"].Add("Paarl");
            groundlocation["Away"].Add("Benoni");
            groundlocation["Away"].Add("Bulawayo");
            groundlocation["Away"].Add("Port of Spain");
            groundlocation["Away"].Add("Kingstown");
            groundlocation["Away"].Add("Bridgetown");
            groundlocation["Away"].Add("Hyderabad (Sind)");
            groundlocation["Away"].Add("Karachi");
            groundlocation["Away"].Add("Lahore");
            groundlocation["Away"].Add("Dhaka");
            groundlocation["Away"].Add("Taupo");
            groundlocation["Away"].Add("Hove");
            groundlocation["Away"].Add("Bristol");
            groundlocation["Away"].Add("Taunton");
            groundlocation["Away"].Add("Birmingham");
            groundlocation["Away"].Add("Galle");
            groundlocation["Away"].Add("Nairobi (Gym)");
            groundlocation["Away"].Add("Lord's");
            groundlocation["Away"].Add("Chester-le-Street");
            groundlocation["Away"].Add("Pietermaritzburg");
            groundlocation["Away"].Add("Rawalpindi");
            groundlocation["Away"].Add("Peshawar");
            groundlocation["Away"].Add("Dambulla");
            groundlocation["Away"].Add("Chittagong");
            groundlocation["Away"].Add("Multan");
            groundlocation["Away"].Add("Kuala Lumpur");
            groundlocation["Away"].Add("Belfast");
            groundlocation["Away"].Add("Southampton");
            groundlocation["Away"].Add("Canberra");
        }
    }

    public class SeriesData
    {
        public String name { get; set; }
        public List<float> data { get; set; }
    }

    public class BattingAverage
    {
        public string BattingScore { get; set; }
        public int BattingTotal { get; set; }
        public int MatchCount { get; set; }
        public int TotalMatches { get; set; }
        public float CumulativeAverage { get; set; }
    }

    public class HighChart
    {
        public Dictionary<string, string> chart { get; set; }
        public Dictionary<string, string> title { get; set; }
        public Dictionary<string, List<string>> xAxis { get; set; }
        public Dictionary<string, Dictionary<string, string>> yAxis { get; set; }
        public List<SeriesData> series { get; set; }

        public HighChart()
        {
            chart = new Dictionary<string, string>();
            title = new Dictionary<string, string>();
            xAxis = new Dictionary<string, List<string>>();
            yAxis = new Dictionary<string, Dictionary<string, string>>();
            series = new List<SeriesData>();
        }
    }
}
