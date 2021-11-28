import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { IAlumnusInfo } from '../../../../models/account/profile/alumnus/alumnus-info.model';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { AlumnusService } from '../../../../services/account/alumnus/alumnus.service';
import { IAlumnusEditInfo } from '../../../../models/account/profile/alumnus/alumnus-edit-info.model';

@Component({
  selector: 'app-alumnus-edit-profile',
  templateUrl: './alumnus-edit-profile.component.html',
  styleUrls: ['./alumnus-edit-profile.component.scss'],
})
export class AlumnusEditProfileComponent implements OnInit {

  date = Date.now();
  info: IAlumnusInfo;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private alumnusService: AlumnusService,
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
    this.alumnusService.getInfo().subscribe(
      (res: IAlumnusInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAlumnusInfo): void {
    this.info = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IAlumnusInfo): void {
    this.editForm = this.fb.group({
      dateOfGraduation: [data.dateOfGraduation, [Validators.maxLength(10)]],
      college: [data.college, [Validators.required, Validators.maxLength(20)]],
      department: [data.department, [Validators.required, Validators.maxLength(20)]],
      class: [data.class, [Validators.required, Validators.maxLength(20)]]
    });
  }

  async onSubmit(data: IAlumnusEditInfo): Promise<void> {
    await this.loadingService.start('修改中...');
    this.alumnusService.editInfo(data).subscribe(
      (res: IAlumnusInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAlumnusInfo): Promise<void> {
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
        await this.router.navigate(['/account/profile/alumnus']);
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
