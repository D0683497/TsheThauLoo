import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoadingService } from '../../../services/loading/loading.service';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IChangePassword } from '../../../models/account/password/change-password.models';
import { MustMatch } from '../../../validators/must-match.validator';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent implements OnInit {

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
      currentPassword: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64),
          Validators.pattern(new RegExp(/(?=.*\d)(?=.*[a-z])/gm))
        ]
      ],
      newPassword: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64),
          Validators.pattern(new RegExp(/(?=.*\d)(?=.*[a-z])/gm))
        ]
      ],
      confirmNewPassword: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64)
        ]
      ],
    }, { validators: MustMatch('newPassword', 'confirmNewPassword') });
  }

  async onSubmit(data: IChangePassword): Promise<void> {
    await this.loadingService.start('修改中...');
    this.accountService.changePassword(data).subscribe(
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
