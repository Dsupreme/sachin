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

  private countryListArr

  constructor(private _sachinSvc: SachinService) {
    this.state = {
      countryPanelOpen: false,
      countryList: {}
    };
  }

  ngOnInit() {
    // Send call in the beginning of the sidebar component load
    this.fetchOpposition();
  }

  fetchOpposition() {
    this._sachinSvc.getOpposition().subscribe(teams => {
      this.state.countryList = teams;
      // Fetch keys in Array to render "vs Country" filter
      this.countryListArr = Object.keys(this.state.countryList);
    });
  }

  fnToggleCountry(e) {
    this.state.countryList[e.value] = e.checked;
    // console.log(this.state.countryList);
    // call update chart functions here
    this.render();
  }

  fnToggleState(panel) {
    this.state[panel] = !this.state[panel];
  }

  render() {
    this._sachinSvc.fetchBattingAverage({
      countries: this.state.countryList
    }).subscribe(data => {
      this.data = data;
      console.log(this.data);
    }
      )
  }

}
