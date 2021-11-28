import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../services/loading/loading.service';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';
import { IChangePhone } from '../../models/account/change-phone.models';
import { PhoneNumberValidator } from '../../validators/phone.validator';

@Component({
  selector: 'app-change-phone',
  templateUrl: './change-phone.component.html',
  styleUrls: ['./change-phone.component.scss'],
})
export class ChangePhoneComponent implements OnInit {

  date = new Date().toISOString();
  changeForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit(): void {
    this.changeForm = this.fb.group({
      newPhoneNumber: [
        null,
        [
          Validators.required,
          Validators.maxLength(30),
          PhoneNumberValidator('TW')
        ]
      ],
    });
  }

  async onSubmit(data: IChangePhone): Promise<void> {
    await this.loadingService.start('修改中...');
    this.accountService.changePhone(data).subscribe(
      () => { this.changeSuccess(); },
      (err: HttpErrorResponse) => { this.changeFail(err); }
    );
  }

  async changeSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.accountService.logout();
    await this.router.navigate(['/account/login']);
    await this.notificationService.notify('修改成功', '請重新登入', SweetAlertIcon.success);
  }

  async changeFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.changeForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
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
