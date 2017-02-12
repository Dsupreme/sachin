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
using sachin.Models;

namespace sachin.Controllers
{

    [RoutePrefix("api")]
    public class SachinController : ApiController
    {
        [HttpGet]
        [Route("versusTeams")]
        public IHttpActionResult versumTeams()
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
                    versus[against] = new int[2] { 0, 0 };
                }

                if (gameScore == "won")
                {
                    versus[against].SetValue(Int32.Parse(versus[against].GetValue(0).ToString()) + 1, 0);
                }
                else
                {
                    versus[against].SetValue(Int32.Parse(versus[against].GetValue(1).ToString()) + 1, 1);
                }
            }

            yAxis.Add("text", " Win vs loss against each Team");
            seriesWin.name = "Win";
            seriesWin.data = new List<float>();
            seriesLoss.name = "Loss";
            seriesLoss.data = new List<float>();
            foreach (string key in versus.Keys)
            {
                var x = versus[key];
                seriesWin.data.Add(Int32.Parse(x.GetValue(0).ToString()));
                seriesLoss.data.Add(Int32.Parse(x.GetValue(1).ToString()));
            }

            h.chart.Add("type", "column");
            h.title.Add("text", "versusTeams");
            h.xAxis.Add("categories", versus.Keys.ToList());
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

            List<Dictionary<string, string>> abc = new List<Dictionary<string, string>>();

            List<BattingAverage> response = new List<BattingAverage>();

            List<string> dateValue = new List<string>();

            // Required
            int count = 0;
            int battingScore;
            string rowBattingScore;
            float battingAverage = 0;

            SeriesData battingScores = new SeriesData();
            SeriesData battingAverages = new SeriesData();

            battingScores.name = "Batting Scores";
            battingScores.data = new List<float>();

            battingAverages.name = "Batting Averages";
            battingAverages.data = new List<float>();



            foreach (DataRow row in dt.Rows)
            {
                count++;
                BattingAverage ele = new BattingAverage();

                rowBattingScore = row["batting_score"].ToString();
                matchDate = row["date"].ToString();

                if (rowBattingScore == "DNB" || rowBattingScore == "TDNB")
                {

                }
                else
                {

                    if (!rowBattingScore.EndsWith("*"))
                    {
                        MatchCount++;
                    }

                    battingScore = Int32.Parse(rowBattingScore.TrimEnd('*'));
                    battingTotal += battingScore;
                    battingAverage = (float)battingTotal / MatchCount;

                    ele.BattingScore = rowBattingScore;
                    ele.BattingTotal = battingTotal;
                    ele.TotalMatches = count;
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

            h.xAxis.Add("categories", dateValue);
            h.series.Add(battingScores);
            h.series.Add(battingAverages);


            return Ok(h);
        }

        [HttpGet]
        [Route("getOpposition")]
        public IHttpActionResult getOpposition()
        {
            DataTable dt = Startup.SachinDB;
            Dictionary<string, bool> teams = new Dictionary<string, bool>();

            foreach (DataRow row in dt.Rows)
            {
                if (!teams.ContainsKey(row["opposition"].ToString().Replace("v ", "")))
                {
                    teams.Add(row["opposition"].ToString().Replace("v ", ""), true);
                }
            }
            return Ok(teams);
        }

        [HttpPost]
        [Route("getBatting")]
        public IHttpActionResult getBatting([FromBody] dynamic request)
        {
            FiltersModel selectedFilters = GetSelectedFilters(request);
            DataTable filteredDt = filterDt(selectedFilters);
            





            //return Ok(BattingAverageCalculator(dt));
            return Ok(CumulativeBattingAverageCalculator(filteredDt));

            //return Ok(h);
        }



        public static HighChart BattingAverageCalculator(DataTable dt)
        {
            HighChart h = new HighChart();


            Dictionary<string, List<string>> years = new Dictionary<string, List<string>>();

            SeriesData battingScores = new SeriesData();
            SeriesData battingAverages = new SeriesData();

            battingScores.name = "Batting Scores";
            battingScores.data = new List<float>();

            battingAverages.name = "Batting Averages";
            battingAverages.data = new List<float>();

            foreach (DataRow row in dt.Rows)
            {
                var matchDate = Convert.ToDateTime(row["date"].ToString()).Year.ToString();
                var battingScore = row["batting_score"].ToString();
                if (!years.ContainsKey(matchDate))
                {
                    years[matchDate] = new List<string>();
                }
                if (!(battingScore == "DNB" || battingScore == "TDNB"))
                {
                    years[matchDate].Add(battingScore);
                }
            }

            foreach (var scores in years.Keys)
            {
                int battingTotal = 0;
                int matchesTotal = 0;
                foreach (var score in years[scores])
                {
                    if (!score.Contains('*'))
                    {
                        matchesTotal++;
                    }
                    battingTotal += Convert.ToInt32(score.TrimEnd('*'));
                }
                battingScores.data.Add(battingTotal);
                battingAverages.data.Add((float)battingTotal / matchesTotal);

                //years[scores].RemoveAll();
                //years[scores].Add(battingTotal.ToString(), matchesTotal.ToString());
            }

            h.xAxis.Add("categories", years.Keys.ToList());
            h.series.Add(battingScores);
            h.series.Add(battingAverages);

            return h;
        }

        public static HighChart CumulativeBattingAverageCalculator(DataTable dt)
        {
            HighChart h = new HighChart();
            Dictionary<string, List<string>> years = new Dictionary<string, List<string>>();

            int battingTotal = 0;
            int matchesTotal = 0;

            SeriesData battingScores = new SeriesData();
            SeriesData battingAverages = new SeriesData();

            battingScores.name = "Batting Scores";
            battingScores.data = new List<float>();

            battingAverages.name = "Batting Averages";
            battingAverages.data = new List<float>();

            foreach (DataRow row in dt.Rows)
            {
                var matchDate = Convert.ToDateTime(row["date"].ToString()).Year.ToString();
                var battingScore = row["batting_score"].ToString();
                if (!years.ContainsKey(matchDate))
                {
                    years[matchDate] = new List<string>();
                }
                if (!(battingScore == "DNB" || battingScore == "TDNB"))
                {
                    years[matchDate].Add(battingScore);
                }
            }

            foreach (var scores in years.Keys)
            {

                foreach (var score in years[scores])
                {
                    if (!score.Contains('*'))
                    {
                        matchesTotal++;
                    }
                    battingTotal += Convert.ToInt32(score.TrimEnd('*'));
                }
                battingScores.data.Add(battingTotal);
                battingAverages.data.Add((float)battingTotal / matchesTotal);

                //years[scores].RemoveAll();
                //years[scores].Add(battingTotal.ToString(), matchesTotal.ToString());
            }

            h.xAxis.Add("categories", years.Keys.ToList());
            h.series.Add(battingScores);
            h.series.Add(battingAverages);

            return h;
        }

        public static FiltersModel GetSelectedFilters(dynamic inputObj)
        {
            FiltersModel selectedFilters = new FiltersModel();
            selectedFilters.selectedCountries = inputObj["countries"];
            selectedFilters.selectedInnings = inputObj["innings"];
            selectedFilters.selectedResult = inputObj["result"];
            selectedFilters.selectedGround = inputObj["ground"];
            return selectedFilters;
        }

        public DataTable filterDt(FiltersModel selectedFilters)
        {
            DataTable dt = Startup.SachinDB.Copy();
            GroundLocationModel grounds = new GroundLocationModel();
            
            List<string> countries = new List<string>();
            List<string> innings = new List<string>();
            List<string> result = new List<string>();
            List<string> ground = new List<string>();
                        
            // Filter Countries
            foreach (var row in selectedFilters.selectedCountries)
            {
                if (row.Value == true)
                {
                    countries.Add("v " + row.Name);
                }
            }

            // Filter batting innings
            foreach (var row in selectedFilters.selectedInnings)
            {
                if (row.Value == true)
                {
                    innings.Add(row.Name);
                }
            }

            foreach (var row in selectedFilters.selectedResult)
            {
                if (row.Value == true)
                {
                    result.Add(row.Name);
                }
            }

            foreach (var row in selectedFilters.selectedGround)
            {
                if (row.Value == true)
                {   
                    ground = ground.Union(grounds.groundlocation[row.Name] as IEnumerable<string>).ToList();
                }
            }


            DataRow[] originaltable = dt.Select();
            foreach (DataRow row in originaltable)
            {
                if (countries.IndexOf(row["opposition"].ToString()) == -1 
                    || innings.IndexOf(row["batting_innings"].ToString()) == -1
                    || result.IndexOf(row["match_result"].ToString()) == -1
                    || ground.IndexOf(row["ground"].ToString()) == -1
                    )
                {
                    dt.Rows.Remove(row);
                }
            }

            return dt;
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


        //BattingAverage ele = new BattingAverage();

        //        rowBattingScore = row["batting_score"].ToString();
        //        matchDate = row["date"].ToString();

        //        if (rowBattingScore == "DNB" || rowBattingScore == "TDNB") {

        //        } else
        //        {

        //            if (!rowBattingScore.EndsWith("*"))
        //            {
        //                MatchCount++;
        //            }

        //            battingScore = Int32.Parse(rowBattingScore.TrimEnd('*'));
        //            battingTotal += battingScore;
        //            battingAverage = (float) battingTotal/MatchCount;

        //            ele.BattingScore = rowBattingScore;
        //            ele.BattingTotal = battingTotal;
        //            ele.TotalMatches= count;
        //            ele.MatchCount = MatchCount;
        //            ele.CumulativeAverage = battingAverage;

        //            dateValue.Add(matchDate);
        //            battingScores.data.Add(battingScore);
        //            battingAverages.data.Add(battingAverage);

        //            //Dictionary<string,string> aaa = new Dictionary<string,string>();
        //            //aaa.Add(count,
        //              //  battingAverage.ToString(),totalMatches.ToString());
        //            //abc.Add(aaa);



        //        }


        //        //response.Add(ele);
        //        //def.Add(totalMatches.ToString());
        //    }      

        //    List<List<string>> pqr = new List<List<string>>();
        //    //pqr.Add(abc);

        //    h.xAxis.Add("categories",dateValue);
        //    h.series.Add(battingScores);
        //    h.series.Add(battingAverages);

        //    //return Ok(h) ;
        //    //return "abcdef";


        //    //HttpResponseMessage x = Request.CreateResponse<string>(System.Net.HttpStatusCode.OK, "string accepted");
        //    //x.AppendHeader("Access-Control-Allow-Origin", "*");
        //    ////x.Headers = new HttpResponseHeader("Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With");
    }
}
