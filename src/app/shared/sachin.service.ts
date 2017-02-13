import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Observable, Subject } from 'rxjs';

import 'rxjs/add/operator/map';

@Injectable()
export class SachinService {
  // static variables
  private baseURL;
  // Observable sources
  private battingAverage
  private versusTeam

  constructor(private _http: Http) {
    this.baseURL = "http://localhost:54094/api/";
    this.battingAverage = new Subject<any>();
    this.versusTeam = new Subject<any>();
  }

  // POST request for Batting Performance TimeLine Chart
  fetchBattingAverage(filters) {
    this._http.post(this.baseURL + 'getBatting', filters)
      .map(res => res.json())
      .subscribe(result => {
        this.battingAverage.next(result);
      })
  }

  // POST request for Versus Teams Chart
  fetchVersusTeams(filters) {
    this._http.post(this.baseURL + 'getVersusTeams', filters)
      .map(res => res.json())
      .subscribe(result => {
        this.versusTeam.next(result);
      })
  }

  // To make observable objects that can be attached to subscribers
  getBattingAverage(): Observable<any> {
    return this.battingAverage.asObservable();
  }

  // To make observable objects that can be attached to subscribers
  getVersusTeam(): Observable<any> {
    return this.versusTeam.asObservable();
  }

  // Fetch 'Versus Country' filter at runtime from API instead of defining as static array
  getOpposition() {
    return this._http.get(this.baseURL + 'getOpposition')
      .map(res => res.json());
  }
}