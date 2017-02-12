import { Component, OnInit } from '@angular/core';

import { SachinService } from '../shared/sachin.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  private state
  private data

  private countryArr
  private inningsArr
  private matchResultArr
  private groundLocationArr

  constructor(private _sachinSvc: SachinService) {
    this.state = {
      countryPanelOpen: false,
      battingInningsPanelOpen: false,
      matchResultPanelOpen: false,
      groundLocationPanelOpen: false,
      countryList: {},
      inningsList: {
        "1st": true,
        "2nd": true
      },
      matchResultList: {
        "won": true,
        "lost": true
      },
      groundLocationList: {
        "Home": true,
        "Away": true
      }
    };

    this.inningsArr = ["1st", "2nd"]
    this.matchResultArr = ["won", "lost"]
    this.groundLocationArr = ["Home", "Away"]
  }

  ngOnInit() {
    // Send call in the beginning of the sidebar component load
    this.fetchOpposition();
  }

  // To fetch the list of countries from Service instead of hard coding in component
  fetchOpposition() {
    this._sachinSvc.getOpposition().subscribe(teams => {
      this.state.countryList = teams;
      // Fetch keys in Array to render "vs Country" filter
      this.countryArr = Object.keys(this.state.countryList);
    });
  }

  // Toggle panel open/close state
  fnToggleState(panel) {
    this.state[panel] = !this.state[panel];
  }

  // Update list of selected countries
  fnToggleCountry(e) {
    this.state.countryList[e.value] = e.checked;
    this.render();
  }

  // Update list of selected batting innings
  fnToggleInnings(e) {
    this.state.inningsList[e.value] = e.checked;
    this.render();
  }

  // Update list of selected match result
  fnToggleMatchResult(e) {
    this.state.matchResultList[e.value] = e.checked;
    this.render();
  }

  // Update list of selected ground venues
  fnToggleGroundLocation(e) {
    this.state.groundLocationList[e.value] = e.checked;
    this.render();
  }

  // Function to render all the charts.
  render() {
    this._sachinSvc.fetchBattingAverage({
      countries: this.state.countryList,
      innings: this.state.inningsList,
      result: this.state.matchResultList,
      ground: this.state.groundLocationList
    });
  }

}
