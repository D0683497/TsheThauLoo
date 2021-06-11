import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { StudentService } from '../../../services/account/student/student.service';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IStudentProfile } from '../../../models/account/profile/student/student-profile.model';

@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.scss'],
})
export class StudentProfileComponent implements OnInit {

  date = Date.now();
  profile: IStudentProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;

  constructor(
    private studentService: StudentService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.studentService.getProfile().subscribe(
      (res: IStudentProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IStudentProfile): void {
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
