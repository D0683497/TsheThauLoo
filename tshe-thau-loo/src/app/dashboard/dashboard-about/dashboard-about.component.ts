import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard-about',
  templateUrl: './dashboard-about.component.html',
  styleUrls: ['./dashboard-about.component.scss'],
})
export class DashboardAboutComponent implements OnInit {

  date = Date.now();

  constructor() { }

  ngOnInit() {}

}
