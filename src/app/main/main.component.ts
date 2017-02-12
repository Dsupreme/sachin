import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';

import { SachinService } from '../shared/sachin.service';


Highcharts.setOptions({
  colors: ['#058DC7', '#50B432', '#ED561B']
});


@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  options: Object
  battingAverage: Object
  private _subscription

  constructor(private _sachinSvc: SachinService) {
    // this.battingAverage = _sachinSvc.battingAverage;
    this._subscription = _sachinSvc.getBattingAverage().subscribe((value) => {
      this.battingAverage = value;
    })



    this.options = {
      "chart": {
        "type": "column"
      },
      "title": {
        "text": "versusTeams"
      },
      "xAxis": {
        "categories": [
          "v Pakistan",
          "v New Zealand",
          "v Sri Lanka",
          "v England",
          "v Bangladesh",
          "v West Indies",
          "v South Africa",
          "v Australia",
          "v Zimbabwe",
          "v U.A.E.",
          "v Kenya",
          "v Netherlands",
          "v Namibia",
          "v Bermuda",
          "v Ireland"
        ]
      },
      "yAxis": {
        "title": {
          "text": " Win vs loss against each Team"
        }
      },
      "series": [
        {
          "name": "Win",
          "data": [
            31,
            22,
            43,
            19,
            10,
            22,
            21,
            23,
            27,
            2,
            8,
            2,
            1,
            1,
            2
          ]
        },
        {
          "name": "Loss",
          "data": [
            38,
            20,
            41,
            18,
            2,
            17,
            36,
            48,
            7,
            0,
            2,
            0,
            0,
            0,
            0
          ]
        }
      ]
    };

  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

}
