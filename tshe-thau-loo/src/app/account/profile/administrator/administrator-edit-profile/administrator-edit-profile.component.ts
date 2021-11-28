import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AdministratorService } from '../../../../services/account/administrator/administrator.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IAdministratorInfo } from '../../../../models/account/profile/administrator/administrator-info.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IAdministratorEditInfo } from '../../../../models/account/profile/administrator/administrator-edit-info.model';
import { LoadingService } from '../../../../services/loading/loading.service';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';

@Component({
  selector: 'app-administrator-edit-profile',
  templateUrl: './administrator-edit-profile.component.html',
  styleUrls: ['./administrator-edit-profile.component.scss'],
})
export class AdministratorEditProfileComponent implements OnInit {

  date = Date.now();
  info: IAdministratorInfo;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  jobTitleValidators = [Validators.maxLength(20)];
  extensionValidators = [Validators.maxLength(10), Validators.pattern('^[0123456789]+$')];
  contactEmailValidators = [Validators.email, Validators.maxLength(320)];

  constructor(
    private administratorService: AdministratorService,
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
    this.administratorService.getInfo().subscribe(
      (res: IAdministratorInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAdministratorInfo): void {
    this.info = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IAdministratorInfo): void {
    this.editForm = this.fb.group({
      showAbout: [data.showAbout, Validators.required],
      networkId: [data.networkId, [Validators.required, Validators.maxLength(10)]],
      dept: [data.dept, [Validators.required, Validators.maxLength(20)]],
      unit: [data.unit, [Validators.maxLength(20)]],
      jobTitle: [data.jobTitle, this.jobTitleValidators],
      extension: [data.extension, this.extensionValidators],
      contactEmail: [data.contactEmail, this.contactEmailValidators]
    });
  }

  async onSubmit(data: IAdministratorEditInfo): Promise<void> {
    await this.loadingService.start('修改中...');
    this.administratorService.editInfo(data).subscribe(
      (res: IAdministratorInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAdministratorInfo): Promise<void> {
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
        await this.router.navigate(['/account/profile/administrator']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  toggleShowAbout(value: any): void {
    if (value.detail.checked) {
      // jobTitle
      this.editForm.get('jobTitle').setValidators([Validators.required, ...this.jobTitleValidators]);
      this.editForm.get('jobTitle').updateValueAndValidity();
      // extension
      this.editForm.get('extension').setValidators([Validators.required, ...this.extensionValidators]);
      this.editForm.get('extension').updateValueAndValidity();
      // contactEmail
      this.editForm.get('contactEmail').setValidators([Validators.required, ...this.contactEmailValidators]);
      this.editForm.get('contactEmail').updateValueAndValidity();
    } else {
      // jobTitle
      this.editForm.get('jobTitle').clearValidators();
      this.editForm.get('jobTitle').setValidators(this.jobTitleValidators);
      this.editForm.get('jobTitle').updateValueAndValidity();
      // extension
      this.editForm.get('extension').clearValidators();
      this.editForm.get('extension').setValidators(this.extensionValidators);
      this.editForm.get('extension').updateValueAndValidity();
      // contactEmail
      this.editForm.get('contactEmail').clearValidators();
      this.editForm.get('contactEmail').setValidators(this.contactEmailValidators);
      this.editForm.get('contactEmail').updateValueAndValidity();
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
