import { Component, OnInit } from '@angular/core';
import * as Highcharts from 'highcharts';
import { Subscription } from 'rxjs';

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
  private versusTeams: Object
  private battingAverage: Object
  private _subscriptionBattingAverage: Subscription
  private _subscriptionVersusTeams: Subscription

  constructor(private _sachinSvc: SachinService) {
    // Subscribe to changes in batting average dataset
    this._subscriptionBattingAverage = this._sachinSvc.getBattingAverage().subscribe(value => {
      this.battingAverage = value
    })
    // Subscribe to changes in versus teams dataset
    this._subscriptionVersusTeams = this._sachinSvc.getVersusTeam().subscribe(value => {
      this.versusTeams = value;
    })
  }

  ngOnInit() {
    // Send initial call for all the charts
    this.render();
  }

  ngOnDestroy() {
    // To prevent memory leaks
    this._subscriptionBattingAverage.unsubscribe();
    this._subscriptionVersusTeams.unsubscribe();
  }

  // Function to render all the charts on initial load.
  render() {
    this._sachinSvc.fetchBattingAverage({
      countries: {},
      innings: {},
      result: {},
      ground: {}
    });
    this._sachinSvc.fetchVersusTeams({
      countries: {},
      innings: {},
      result: {},
      ground: {}
    });
  }

}
