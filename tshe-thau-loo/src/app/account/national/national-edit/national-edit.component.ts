import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { INational } from '../../../models/account/national/national.model';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { INationalEdit } from '../../../models/account/national/national-edit.model';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { NationalIdValidator } from '../../../validators/taiwan-id.validator';

@Component({
  selector: 'app-national-edit',
  templateUrl: './national-edit.component.html',
  styleUrls: ['./national-edit.component.scss'],
})
export class NationalEditComponent implements OnInit {

  date = new Date().toISOString();
  national: INational;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
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
    this.accountService.getNational().subscribe(
      (res: INational) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: INational): void {
    this.national = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: INational): void {
    this.editForm = this.fb.group({
      nationalId: [
        {value: data.nationalId, disabled: data.identityConfirmed},
        [Validators.maxLength(10), NationalIdValidator()]
      ],
      name: [
        {value: data.name, disabled: data.identityConfirmed},
        [Validators.required, Validators.maxLength(50)]
      ],
      gender: [{value: data.gender, disabled: data.identityConfirmed},],
      dateOfBirth: [{value: data.dateOfBirth, disabled: data.identityConfirmed},],
      currentAddress: [data.currentAddress, [Validators.maxLength(200)]],
    });
  }

  async onSubmit(data: INationalEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.accountService.editNational(data).subscribe(
      (res: INational) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: INational): Promise<void> {
    await this.loadingService.end();
    this.national = res;
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

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
