import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { NationalIdValidator } from '../../validators/taiwan-id.validator';
import { ActivatedRoute } from '@angular/router';
import { LoadingService } from '../../services/loading/loading.service';
import { NotificationService } from '../../services/notification/notification.service';
import { HttpErrorResponse } from '@angular/common/http';
import { PhoneNumberValidator } from '../../validators/phone.validator';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';
import { IAlumnus } from '../../models/manage/alumnus.models';
import { AlumnusService } from '../../services/manage/alumnus.service';
import { IAlumnusManage } from '../../models/manage/alumnus-manage.models';

@Component({
  selector: 'app-alumnus-manage-edit',
  templateUrl: './alumnus-manage-edit.component.html',
  styleUrls: ['./alumnus-manage-edit.component.scss'],
})
export class AlumnusManageEditComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  alumnus: IAlumnus;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  nationalIdValidators = [Validators.maxLength(10), NationalIdValidator()];
  genderValidators = [];
  dateOfBirthValidators = [];

  constructor(
    private alumnusService: AlumnusService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.alumnusService.getAlumnus(this.userId).subscribe(
      (res: IAlumnus) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IAlumnus): Promise<void> {
    this.alumnus = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IAlumnus): void {
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
      // 校友資料
      dateOfGraduation: [data.dateOfGraduation, [Validators.maxLength(10)]],
      college: [data.college, [Validators.required, Validators.maxLength(20)]],
      department: [data.department, [Validators.required, Validators.maxLength(20)]],
      class: [data.class, [Validators.required, Validators.maxLength(20)]]
    });
  }

  async onSubmit(data: IAlumnusManage): Promise<void> {
    await this.loadingService.start('修改中...');
    this.alumnusService.editAlumnus(this.userId, data).subscribe(
      (res: IAlumnus) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAlumnus): Promise<void> {
    await this.loadingService.end();
    this.alumnus = res;
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

}
