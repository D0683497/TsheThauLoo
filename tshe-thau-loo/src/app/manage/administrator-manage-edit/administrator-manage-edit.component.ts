import { Component, OnInit } from '@angular/core';
import { IAdministrator } from '../../models/manage/administrator.models';
import { BehaviorSubject } from 'rxjs';
import { AdministratorService } from '../../services/manage/administrator.service';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../services/loading/loading.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';
import { NotificationService } from '../../services/notification/notification.service';
import { IAdministratorManage } from '../../models/manage/administrator-manage.models';
import { PhoneNumberValidator } from '../../validators/phone.validator';
import { NationalIdValidator } from '../../validators/taiwan-id.validator';

@Component({
  selector: 'app-administrator-manage-edit',
  templateUrl: './administrator-manage-edit.component.html',
  styleUrls: ['./administrator-manage-edit.component.scss'],
})
export class AdministratorManageEditComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  administrator: IAdministrator;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  nationalIdValidators = [Validators.maxLength(10), NationalIdValidator()];
  genderValidators = [];
  dateOfBirthValidators = [];
  jobTitleValidators = [Validators.maxLength(20)];
  extensionValidators = [Validators.maxLength(10), Validators.pattern('^[0123456789]+$')];
  contactEmailValidators = [Validators.email, Validators.maxLength(320)];

  constructor(
    private administratorService: AdministratorService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.administratorService.getAdministrator(this.userId).subscribe(
      (res: IAdministrator) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IAdministrator): Promise<void> {
    this.administrator = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IAdministrator): void {
    this.editForm = this.fb.group({
      // 基本資料
      userName: [
        data.userName,
        [
          Validators.required,
          Validators.maxLength(100),
          Validators.pattern('^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+\\-=_.]+$')
        ]
      ],
      email: [data.email, [Validators.required, Validators.email, Validators.maxLength(320)]],
      emailConfirmed: [data.emailConfirmed, Validators.required],
      phoneNumber: [data.phoneNumber, [Validators.maxLength(30), PhoneNumberValidator('TW')]],
      phoneNumberConfirmed: [data.phoneNumberConfirmed, Validators.required],
      lockoutEnabled: [data.lockoutEnabled, Validators.required],
      isEnable: [data.isEnable, Validators.required],
      // 個人資料
      identityConfirmed: [data.identityConfirmed, Validators.required],
      nationalId: [data.nationalId, this.nationalIdValidators],
      name: [data.name, [Validators.required, Validators.maxLength(50)]],
      gender: [data.gender, this.genderValidators],
      dateOfBirth: [data.dateOfBirth, this.dateOfBirthValidators],
      currentAddress: [data.currentAddress, [Validators.maxLength(200)]],
      // 管理員資料
      showAbout: [data.showAbout, Validators.required],
      networkId: [data.networkId, [Validators.required, Validators.maxLength(10)]],
      dept: [data.dept, [Validators.required, Validators.maxLength(20)]],
      unit: [data.unit, [Validators.maxLength(20)]],
      jobTitle: [data.jobTitle, this.jobTitleValidators],
      extension: [data.extension, this.extensionValidators],
      contactEmail: [data.contactEmail, this.contactEmailValidators]
    });
  }

  async onSubmit(data: IAdministratorManage): Promise<void> {
    await this.loadingService.start('修改中...');
    this.administratorService.editAdministrator(this.userId, data).subscribe(
      (res: IAdministrator) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAdministrator): Promise<void> {
    await this.loadingService.end();
    this.administrator = res;
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('修改失敗', SweetAlertIcon.error);
    }
  }

  toggleIdentityConfirmed(value: any): void {
    if (value.detail.checked) {
      // nationalId
      this.editForm.get('nationalId').setValidators([Validators.required, ...this.nationalIdValidators]);
      this.editForm.get('nationalId').updateValueAndValidity();
      // gender
      this.editForm.get('gender').setValidators([Validators.required, ...this.genderValidators]);
      this.editForm.get('gender').updateValueAndValidity();
      // dateOfBirth
      this.editForm.get('dateOfBirth').setValidators([Validators.required, ...this.dateOfBirthValidators]);
      this.editForm.get('dateOfBirth').updateValueAndValidity();
    } else {
      // nationalId
      this.editForm.get('nationalId').clearValidators();
      this.editForm.get('nationalId').setValidators(this.nationalIdValidators);
      this.editForm.get('nationalId').updateValueAndValidity();
      // gender
      this.editForm.get('gender').clearValidators();
      this.editForm.get('gender').setValidators(this.genderValidators);
      this.editForm.get('gender').updateValueAndValidity();
      // dateOfBirth
      this.editForm.get('dateOfBirth').clearValidators();
      this.editForm.get('dateOfBirth').setValidators(this.dateOfBirthValidators);
      this.editForm.get('dateOfBirth').updateValueAndValidity();
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

}
