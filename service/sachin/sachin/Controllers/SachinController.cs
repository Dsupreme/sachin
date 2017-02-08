using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;
using System.Web.Http.Cors;

namespace sachin.Controllers
{

    [RoutePrefix("api")]
    public class SachinController : ApiController
    {
        [HttpGet]
        [Route("versusTeams")]
        public IHttpActionResult  versumTeams()
        {
            DataTable dt = Startup.SachinDB;
            Dictionary<string, Array> versus = new Dictionary<string, Array>();
            HighChart h = new HighChart();
            Dictionary<string, string> yAxis = new Dictionary<string, string>();
            SeriesData seriesWin = new SeriesData();
            SeriesData seriesLoss = new SeriesData();

            foreach (DataRow row in dt.Rows)
            {
                var against = row["opposition"].ToString();
                var gameScore = row["match_result"].ToString().Trim();
                if (!versus.ContainsKey(against))
                {
                    versus[against] = new int[2] {0, 0 };
                }

                if (gameScore == "won")
                {
                    versus[against].SetValue(Int32.Parse(versus[against].GetValue(0).ToString())+1,0);
                } else {
                    versus[against].SetValue(Int32.Parse(versus[against].GetValue(1).ToString())+1,1);
                }
            }

            yAxis.Add("text", " Win vs loss against each Team");
            seriesWin.name = "Win";
            seriesWin.data = new List<double>();
            seriesLoss.name = "Loss";
            seriesLoss.data = new List<double>();
            foreach (string key in versus.Keys)
            {
                var x = versus[key];
                seriesWin.data.Add(Int32.Parse(x.GetValue(0).ToString()));
                seriesLoss.data.Add(Int32.Parse(x.GetValue(1).ToString()));
            }

            h.chart.Add("type","column");
            h.title.Add("text", "versusTeams");
            h.xAxis.Add("categories",versus.Keys.ToList());
            h.yAxis.Add("title", yAxis);
            h.series.Add(seriesWin);
            h.series.Add(seriesLoss);
            
            return Ok(h);
        }

        [HttpGet]
        [Route("battingAverage")]
        public IHttpActionResult battingAverage()
        {
            DataTable dt = Startup.SachinDB;
            HighChart h = new HighChart();
            
            int MatchCount = 0;
            int battingTotal = 0;
            

            string matchDate;
            
            List<Dictionary<string,string>> abc = new List<Dictionary<string, string>>();

            List<BattingAverage> response = new List<BattingAverage>();
            
            List<string> dateValue = new List<string>();

            // Required
            int count = 0;
            int battingScore;
            string rowBattingScore;
            double battingAverage = 0;

            SeriesData battingScores = new SeriesData();
            SeriesData battingAverages = new SeriesData();

            battingScores.name = "Batting Scores";
            battingScores.data = new List<double>();

            battingAverages.name = "Batting Averages";
            battingAverages.data = new List<double>();

             

            foreach (DataRow row in dt.Rows)
            {
                count++;
                BattingAverage ele = new BattingAverage();

                rowBattingScore = row["batting_score"].ToString();
                matchDate = row["date"].ToString();

                if (rowBattingScore == "DNB" || rowBattingScore == "TDNB") {

                } else
                {
                    
                    if (!rowBattingScore.EndsWith("*"))
                    {
                        MatchCount++;
                    }

                    battingScore = Int32.Parse(rowBattingScore.TrimEnd('*'));
                    battingTotal += battingScore;
                    battingAverage = (double) battingTotal/MatchCount;
                    
                    ele.BattingScore = rowBattingScore;
                    ele.BattingTotal = battingTotal;
                    ele.TotalMatches= count;
                    ele.MatchCount = MatchCount;
                    ele.CumulativeAverage = battingAverage;

                    dateValue.Add(matchDate);
                    battingScores.data.Add(battingScore);
                    battingAverages.data.Add(battingAverage);

                    //Dictionary<string,string> aaa = new Dictionary<string,string>();
                    //aaa.Add(count,
                      //  battingAverage.ToString(),totalMatches.ToString());
                    //abc.Add(aaa);

                    
                    
                }
               
                
                //response.Add(ele);
                //def.Add(totalMatches.ToString());
            }      
            
            List<List<string>> pqr = new List<List<string>>();
            //pqr.Add(abc);
            
            h.xAxis.Add("categories",dateValue);
            h.series.Add(battingScores);
            h.series.Add(battingAverages);
            
            
            return Ok(h);
        }

        [HttpGet]
        [Route("getOpposition")]
        public IHttpActionResult getOpposition()
        {
            DataTable dt = Startup.SachinDB;
            Dictionary<string,bool> teams = new Dictionary<string,bool>();

            foreach(DataRow row in dt.Rows)
            {   
                if (!teams.ContainsKey(row["opposition"].ToString().TrimStart('v').TrimStart(' ')))
                {
                    teams.Add(row["opposition"].ToString().TrimStart('v').TrimStart(' '), true);
                }
            }
            return Ok(teams);
        }

        [HttpPost]
        [Route("getBatting")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpOptions]
        [AcceptVerbs("POST", "OPTIONS")]
        public HttpResponseMessage getBatting(HttpRequestMessage request)
        {
            var r = request.Content.ReadAsStringAsync().Result;
            
            DataTable dt = Startup.SachinDB;
            HighChart h = new HighChart();
            
            int MatchCount = 0;
            int battingTotal = 0;
            

            string matchDate;
            
            List<Dictionary<string,string>> abc = new List<Dictionary<string, string>>();

            List<BattingAverage> response = new List<BattingAverage>();
            
            List<string> dateValue = new List<string>();

            // Required
            int count = 0;
            int battingScore;
            string rowBattingScore;
            double battingAverage = 0;

            SeriesData battingScores = new SeriesData();
            SeriesData battingAverages = new SeriesData();

            battingScores.name = "Batting Scores";
            battingScores.data = new List<double>();

            battingAverages.name = "Batting Averages";
            battingAverages.data = new List<double>();

             

            foreach (DataRow row in dt.Rows)
            {
                count++;
                BattingAverage ele = new BattingAverage();

                rowBattingScore = row["batting_score"].ToString();
                matchDate = row["date"].ToString();

                if (rowBattingScore == "DNB" || rowBattingScore == "TDNB") {

                } else
                {
                    
                    if (!rowBattingScore.EndsWith("*"))
                    {
                        MatchCount++;
                    }

                    battingScore = Int32.Parse(rowBattingScore.TrimEnd('*'));
                    battingTotal += battingScore;
                    battingAverage = (double) battingTotal/MatchCount;
                    
                    ele.BattingScore = rowBattingScore;
                    ele.BattingTotal = battingTotal;
                    ele.TotalMatches= count;
                    ele.MatchCount = MatchCount;
                    ele.CumulativeAverage = battingAverage;

                    dateValue.Add(matchDate);
                    battingScores.data.Add(battingScore);
                    battingAverages.data.Add(battingAverage);

                    //Dictionary<string,string> aaa = new Dictionary<string,string>();
                    //aaa.Add(count,
                      //  battingAverage.ToString(),totalMatches.ToString());
                    //abc.Add(aaa);

                    
                    
                }
               
                
                //response.Add(ele);
                //def.Add(totalMatches.ToString());
            }      
            
            List<List<string>> pqr = new List<List<string>>();
            //pqr.Add(abc);
            
            h.xAxis.Add("categories",dateValue);
            h.series.Add(battingScores);
            h.series.Add(battingAverages);

            //return Ok(h) ;
            //return "abcdef";

            return Request.CreateResponse<string>(System.Net.HttpStatusCode.OK, "string accepted");
            

            //return Ok(h);
        }

        public class HighChart
        {
            public Dictionary<string, string> chart { get; set; }
            public Dictionary<string, string> title { get; set; }
            public Dictionary<string,List<string>> xAxis { get; set; } 
            public Dictionary<string, Dictionary<string, string>> yAxis { get; set; }
            public List<SeriesData> series {get; set; }
            public HighChart()
            {
                chart = new Dictionary<string, string>();
                title = new Dictionary<string, string>();
                xAxis = new Dictionary<string, List<string>>();
                yAxis = new Dictionary<string, Dictionary<string, string>>();
                series = new List<SeriesData>();
            }
        }

        public class SeriesData
        {
            public String name {get; set; }
            public List<double> data {get; set; }
        }

        public class BattingAverage
        {
            public string BattingScore {get; set; }
            public int BattingTotal {get; set; }
            public int MatchCount {get; set; }
            public int TotalMatches {get; set; }
            public double CumulativeAverage {get; set; }


        }


        
            //dynamic chartConfig = new JObject();
            //dynamic charttype = new JObject();
            //dynamic xAxis = new JArray(versus.Keys);
        
            //charttype.type = "bar";
            //chartConfig["chart"] = charttype;

            //chartConfig["xAxis"] = new JObject();
            //chartConfig["xAxis"]["categories"] = xAxis;

            //chartConfig["yAxis"] = new JObject();
            //chartConfig["yAxis"]["categories"] = new JObject();
            //chartConfig["yAxis"]["categories"]["title"] = "Win vs Loss against each Team";


            //chartConfig["series"] = new JArray();
            //chartConfig["series"] = new JArray(versus.ToList());

            //dynamic Chart = new KeyValuePair<string, dynamic>();
            //Chart.type = "bar";
            //Chart.xAxis = xAxis;


            // chartConfig


            //        Dictionary<string, List<object>> versus = new Dictionary<string, List<object>>();

            //foreach (DataRow row in dt.Rows)
            //{
            //    var against = row["opposition"].ToString();
            //    if (!versus.ContainsKey(against))
            //    {
            //        versus[against] = new List<object>();
            //    }
            //    versus[against].Add(row);
                
            //}

        // List<object> response = new List<object>();


        //foreach(DataRow row in dt.Select("stumps <> '0'"))
        //    {
        //        response.Add(row);
        //    }

                    //response = JsonConvert.SerializeObject(h, Formatting.Indented);
    }
}
