import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth/auth.service';
import { RoleType } from './enums/role-type.enum';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit {

  type = RoleType;

  constructor(public authService: AuthService) {}

  ngOnInit(): void {}

}
