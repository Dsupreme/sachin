import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class SachinService {
  private baseURL;
  private battingAverage = {};

  constructor(private _http: Http) {
    this.baseURL = "http://localhost:54094/api/";
  }

  getData() {
    console.log("connected");
  }

  getOpposition() {
    return this._http.get(this.baseURL + 'getOpposition')
      .map(res => res.json());
  }

  fetchBattingAverage(filters) {

    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });


    // JSON.stringify({ "abc": "def" })
    return this._http.post(
      this.baseURL + 'getBatting',
      JSON.stringify(filters),
      options
    )
      .map(res => res.json());
  }
}
