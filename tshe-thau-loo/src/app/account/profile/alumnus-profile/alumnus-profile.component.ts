import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IAlumnusProfile } from '../../../models/account/profile/alumnus-profile.model';
import { AlumnusService } from '../../../services/account/alumnus/alumnus.service';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';

@Component({
  selector: 'app-alumnus-profile',
  templateUrl: './alumnus-profile.component.html',
  styleUrls: ['./alumnus-profile.component.scss'],
})
export class AlumnusProfileComponent implements OnInit {

  date = Date.now();
  profile: IAlumnusProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private alumnusService: AlumnusService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit() {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.alumnusService.getProfile().subscribe(
      (res: IAlumnusProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAlumnusProfile): void {
    this.profile = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  toggleNationalId = (): boolean => this.showNationalId = !this.showNationalId;

}
