import { Component, OnInit } from '@angular/core';
import { IExaminerProfile } from '../../../../models/account/profile/examiner/examiner-profile.model';
import { BehaviorSubject } from 'rxjs';
import { ExaminerService } from '../../../../services/account/examiner/examiner.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';

@Component({
  selector: 'app-examiner-profile',
  templateUrl: './examiner-profile.component.html',
  styleUrls: ['./examiner-profile.component.scss'],
})
export class ExaminerProfileComponent implements OnInit {

  date = Date.now();
  profile: IExaminerProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private examinerService: ExaminerService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.examinerService.getProfile().subscribe(
      (res: IExaminerProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IExaminerProfile): void {
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
