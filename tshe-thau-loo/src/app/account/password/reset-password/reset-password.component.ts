import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../../services/loading/loading.service';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MustMatch } from '../../../validators/must-match.validator';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IResetPassword } from '../../../models/account/password/reset-password.models';
import { HttpErrorResponse } from '@angular/common/http';
import { IFormError } from '../../../models/error/form-error.model';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {

  date = Date.now();
  resetForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private route: ActivatedRoute) { }

  async ngOnInit(): Promise<void> {
    this.resetForm = this.fb.group({
      userId: [null],
      token: [null],
      password: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64),
          Validators.pattern(new RegExp(/(?=.*\d)(?=.*[a-z])/gm))
        ]
      ],
      passwordConfirm: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64)
        ]
      ]
    }, { validators: MustMatch('password', 'passwordConfirm') });
    if (this.route.snapshot.queryParamMap.has('userId') && this.route.snapshot.queryParamMap.has('token')) {
      const userId = decodeURIComponent(this.route.snapshot.queryParamMap.get('userId') as string);
      const token = decodeURIComponent(this.route.snapshot.queryParamMap.get('token') as string);
      this.resetForm.get('userId').setValue(userId);
      this.resetForm.get('token').setValue(token);
    } else {
      await this.notificationService.message('網址錯誤', SweetAlertIcon.error);
    }
  }

  async onSubmit(data: IResetPassword): Promise<void> {
    await this.loadingService.start('請稍候...');
    this.accountService.resetPassword(data).subscribe(
      () => { this.resetSuccess(); },
      (err: HttpErrorResponse) => { this.resetFail(err); }
    );
  }

  async resetSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/account/login']);
    await this.notificationService.notify('重設成功', '請重新登入', SweetAlertIcon.success);
  }

  async resetFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.resetForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('重設失敗', SweetAlertIcon.error);
    }
  }

}
