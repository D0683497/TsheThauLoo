import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { IExaminerInfo } from '../../../../models/account/profile/examiner/examiner-info.model';
import { ExaminerService } from '../../../../services/account/examiner/examiner.service';
import { IExaminerEditInfo } from '../../../../models/account/profile/examiner/examiner-edit-info.model';

@Component({
  selector: 'app-examiner-edit-profile',
  templateUrl: './examiner-edit-profile.component.html',
  styleUrls: ['./examiner-edit-profile.component.scss'],
})
export class ExaminerEditProfileComponent implements OnInit {

  date = Date.now();
  info: IExaminerInfo;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private examinerService: ExaminerService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.examinerService.getInfo().subscribe(
      (res: IExaminerInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IExaminerInfo): void {
    this.info = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IExaminerInfo): void {
    this.editForm = this.fb.group({
      divisionName: [data.divisionName, [Validators.maxLength(30)]],
      jobTitle: [data.jobTitle, [Validators.maxLength(30)]]
    });
  }

  async onSubmit(data: IExaminerEditInfo): Promise<void> {
    await this.loadingService.start('修改中...');
    this.examinerService.editInfo(data).subscribe(
      (res: IExaminerInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IExaminerInfo): Promise<void> {
    await this.loadingService.end();
    this.info = res;
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('修改失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/examiner']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
