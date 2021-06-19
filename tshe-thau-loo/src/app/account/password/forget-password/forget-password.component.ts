import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../../services/loading/loading.service';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { IForgetPassword } from '../../../models/account/password/forget-password.models';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.scss'],
})
export class ForgetPasswordComponent implements OnInit {

  date = Date.now();
  forgetForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router) { }

  ngOnInit(): void {
    this.forgetForm = this.fb.group({
      email: [null, [Validators.required, Validators.email, Validators.maxLength(320)]]
    });
  }

  async onSubmit(data: IForgetPassword): Promise<void> {
    await this.loadingService.start('請稍候...');
    this.accountService.forgetPassword(data).subscribe(
      () => { this.forgetSuccess(); },
      (err: HttpErrorResponse) => { this.forgetFail(err); }
    );
  }

  async forgetSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/account/login']);
    await this.notificationService.notify('送出成功', '請前往您的信箱收取重設密碼驗證信', SweetAlertIcon.success);
  }

  async forgetFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.forgetForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('發生錯誤', SweetAlertIcon.error);
    }
  }

}
