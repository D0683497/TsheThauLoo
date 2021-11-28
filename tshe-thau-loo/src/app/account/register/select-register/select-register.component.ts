import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-select-register',
  templateUrl: './select-register.component.html',
  styleUrls: ['./select-register.component.scss'],
})
export class SelectRegisterComponent implements OnInit {

  date = Date.now();

  constructor() { }

  ngOnInit() {}

}
