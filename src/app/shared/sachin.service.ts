import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable, Subject } from 'rxjs';
import 'rxjs/add/operator/map';
import * as Highcharts from 'highcharts';

@Injectable()
export class SachinService {
  private baseURL;
  private battingAverage
  private battingAverage$ = new Subject<any>()



  constructor(private _http: Http) {
    this.baseURL = "http://localhost:54094/api/";
  }

  getOpposition() {
    return this._http.get(this.baseURL + 'getOpposition')
      .map(res => res.json());
  }

  fetchBattingAverage(filters) {
    this.battingAverage = this._http.post(this.baseURL + 'getBatting', filters)
      .map(res => res.json())
    this.setbattingAverage();
  }

  setbattingAverage() {
    this.battingAverage$.next(this.battingAverage)
  }

  getBattingAverage(): Observable<any> {
    return this.battingAverage$.asObservable();
  }

  // this.battingAverage = this._http.post(this.baseURL + 'getBatting', filters)
  //   .map(res => res.json());
  // this.battingAverageChange.next(this.battingAverage);
  // console.log(this.battingAverage);


}



// private baseURL;
//   public battingAverage

//   battingAverageChange: Subject<string> = new Subject<string>()

//   battingAverageChange$ = this.battingAverageChange.asObservable();

//   constructor(private _http: Http) {
//     this.baseURL = "http://localhost:54094/api/";
//     this.battingAverage = {
//       "chart": {},
//       "title": {
//         "text": "Batting Performance"
//       },
//       "xAxis": {
//         "crosshair": true,
//         "categories": [
//           "1989",
//           "1990",
//           "1991",
//           "1992",
//           "1993",
//           "1994",
//           "1995",
//           "1996",
//           "1997",
//           "1998",
//           "1999",
//           "2000",
//           "2001",
//           "2002",
//           "2003",
//           "2004",
//           "2005",
//           "2006",
//           "2007",
//           "2008",
//           "2009",
//           "2010",
//           "2011",
//           "2012"
//         ]
//       },
//       "yAxis": [{ // Primary yAxis
//         title: {
//           text: 'Batting Average',
//           style: {
//             color: Highcharts.getOptions().colors[1]
//           }
//         },
//         labels: {
//           format: '{value} runs',
//           style: {
//             color: Highcharts.getOptions().colors[1]
//           }
//         },

//         opposite: true

//       }, { // Secondary yAxis
//         gridLineWidth: 0,
//         title: {
//           text: 'Runs Scored',
//           style: {
//             color: Highcharts.getOptions().colors[0]
//           }
//         },
//         labels: {
//           format: '{value} runs',
//           style: {
//             color: Highcharts.getOptions().colors[0]
//           }
//         }

//       }],
//       tooltip: {
//         shared: true
//       },
//       "series": [{
//         name: 'Batting Score',
//         type: 'line',
//         yAxis: 1,
//         "data": [
//           0,
//           203,
//           620,
//           1240,
//           1559,
//           2271,
//           2560,
//           4171,
//           5065,
//           6828,
//           7361,
//           8620,
//           9524,
//           10265,
//           11225,
//           12037,
//           12449,
//           13077,
//           14502,
//           14962,
//           15644,
//           15848,
//           16361,
//           16676
//         ],
//         tooltip: {
//           valueSuffix: ' run(s)'
//         }
//       }, {
//         "name": "Batting Averages",
//         "data": [
//           0,
//           22.5555553,
//           29.52381,
//           32.63158,
//           30.5686283,
//           33.39706,
//           35.0684929,
//           40.4951439,
//           37.51852,
//           42.40994,
//           42.7965126,
//           42.0487823,
//           43.6880722,
//           44.24569,
//           45.63008,
//           45.25188,
//           44.30249,
//           44.3288155,
//           44.62154,
//           44.5297623,
//           44.5698,
//           45.022728,
//           45.0716248,
//           44.7077751
//         ],
//         yAxis: 0,
//         tooltip: {
//           valueSuffix: ' run(s)'
//         }
//       }]
//     };
//   }

//   getData() {
//     console.log("connected");
//   }

//   getOpposition() {
//     return this._http.get(this.baseURL + 'getOpposition')
//       .map(res => res.json());
//   }

//   fetchBattingAverage(filters) {
//     this.battingAverage = this._http.post(this.baseURL + 'getBatting', filters)
//       .map(res => res.json());
//     this.battingAverageChange.next(this.battingAverage);
//     console.log(this.battingAverage);
//   }
