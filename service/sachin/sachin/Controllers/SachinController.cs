using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using sachin.Models;
using System.Configuration;

namespace sachin.Controllers
{

    [RoutePrefix("api")]
    public class SachinController : ApiController
    {
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
        [Route("getVersusTeams")]
        public IHttpActionResult versumTeams([FromBody] dynamic request)
        {
            FiltersModel selectedFilters = GetSelectedFilters(request);
            DataTable dt = filterDt(selectedFilters);

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
            h.title.Add("text", "Versus Teams");
            h.xAxis.Add("categories", versus.Keys.ToList());
            h.yAxis.Add("title", yAxis);
            h.series.Add(seriesWin);
            h.series.Add(seriesLoss);

            return Ok(h);
        }

        [HttpPost]
        [Route("getBatting")]
        public IHttpActionResult getBatting([FromBody] dynamic request)
        {
            FiltersModel selectedFilters = GetSelectedFilters(request);
            DataTable filteredDt = filterDt(selectedFilters);
            // @"D:\Learning\socialCops\service\sachin\battingAverage.json"
            JToken response = JToken.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["json"])));

            HighChart h = CumulativeBattingAverageCalculator(filteredDt);
            response["xAxis"]["categories"] = JToken.FromObject(h.xAxis["categories"]);
            JToken data1 = JToken.FromObject(h.series[0]);
            JToken data2 = JToken.FromObject(h.series[1]);

            response["series"][0]["data"] = JToken.FromObject(data1["data"]);
            response["series"][1]["data"] = JToken.FromObject(data2["data"]);

            return Ok(response);
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
                int flag = 1;
                if (countries.Count > 0)
                {
                    if (countries.IndexOf(row["opposition"].ToString()) == -1)
                    {
                        flag = 0;
                    }
                }
                if (innings.Count > 0)
                {
                    if (innings.IndexOf(row["batting_innings"].ToString()) == -1)
                    {
                        flag = 0;
                    }
                }
                if (result.Count > 0)
                {
                    if (result.IndexOf(row["match_result"].ToString()) == -1)
                    {
                        flag = 0;
                    }
                }
                if (ground.Count > 0)
                {
                    if (ground.IndexOf(row["ground"].ToString()) == -1)
                    {
                        flag = 0;
                    }
                }
                if (flag == 0)
                {
                    dt.Rows.Remove(row);
                }
            }

            return dt;
        }

    }
}
