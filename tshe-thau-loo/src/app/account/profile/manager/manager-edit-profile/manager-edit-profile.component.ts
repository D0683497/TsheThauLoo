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
import { IManagerInfo } from '../../../../models/account/profile/manager/manager-info.model';
import { ManagerService } from '../../../../services/account/manager/manager.service';
import { IManagerEditInfo } from '../../../../models/account/profile/manager/manager-edit-info.model';
import { PhoneNumberValidator } from '../../../../validators/phone.validator';

@Component({
  selector: 'app-manager-edit-profile',
  templateUrl: './manager-edit-profile.component.html',
  styleUrls: ['./manager-edit-profile.component.scss'],
})
export class ManagerEditProfileComponent implements OnInit {

  date = Date.now();
  info: IManagerInfo;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private managerService: ManagerService,
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
    this.managerService.getInfo().subscribe(
      (res: IManagerInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IManagerInfo): void {
    this.info = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IManagerInfo): void {
    this.editForm = this.fb.group({
      divisionName: [data.divisionName, [Validators.required, Validators.maxLength(30)]],
      jobTitle: [data.jobTitle, [Validators.required, Validators.maxLength(30)]],
      contactEmail: [data.contactEmail, [Validators.required, Validators.email, Validators.maxLength(320)]],
      contactPhone: [
        data.contactPhone,
        [
          Validators.required,
          Validators.maxLength(30),
          PhoneNumberValidator('TW')
        ]
      ],
      contactAddress: [data.contactAddress, [Validators.required, Validators.maxLength(200)]]
    });
  }

  async onSubmit(data: IManagerEditInfo): Promise<void> {
    await this.loadingService.start('修改中...');
    this.managerService.editInfo(data).subscribe(
      (res: IManagerInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IManagerInfo): Promise<void> {
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
        await this.router.navigate(['/account/profile/manager']);
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
