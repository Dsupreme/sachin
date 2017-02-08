import { Component } from '@angular/core';

import { SachinService } from "./shared/sachin.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private sachinService: SachinService) { }

  title = 'app works!';
}
