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
import { IExaminer } from '../../models/manage/examiner.models';
import { ExaminerService } from '../../services/manage/examiner.service';
import { IExaminerManage } from '../../models/manage/examiner-manage.models';

@Component({
  selector: 'app-examiner-manage-edit',
  templateUrl: './examiner-manage-edit.component.html',
  styleUrls: ['./examiner-manage-edit.component.scss'],
})
export class ExaminerManageEditComponent implements OnInit {

  date = Date.now();
  userId = this.route.snapshot.paramMap.get('userId');
  examiner: IExaminer;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  nationalIdValidators = [Validators.maxLength(10), NationalIdValidator()];
  genderValidators = [];
  dateOfBirthValidators = [];

  constructor(
    private examinerService: ExaminerService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.examinerService.getExaminer(this.userId).subscribe(
      (res: IExaminer) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IExaminer): Promise<void> {
    this.examiner = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IExaminer): void {
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
      // 審查員資料
      divisionName: [data.divisionName, [Validators.maxLength(30)]],
      jobTitle: [data.jobTitle, [Validators.maxLength(30)]]
    });
  }

  async onSubmit(data: IExaminerManage): Promise<void> {
    await this.loadingService.start('修改中...');
    this.examinerService.editExaminer(this.userId, data).subscribe(
      (res: IExaminer) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IExaminer): Promise<void> {
    await this.loadingService.end();
    this.examiner = res;
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
