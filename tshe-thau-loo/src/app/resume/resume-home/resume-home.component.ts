import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-resume-home',
  templateUrl: './resume-home.component.html',
  styleUrls: ['./resume-home.component.scss'],
})
export class ResumeHomeComponent implements OnInit {

  date = Date.now();

  constructor() { }

  ngOnInit(): void {}

}
