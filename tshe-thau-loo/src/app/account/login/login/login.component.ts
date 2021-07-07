import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account/account.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { ILogin } from '../../../models/account/login/login.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  date = Date.now();
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userName: [
        null,
        [
          Validators.required,
          Validators.maxLength(100),
          Validators.pattern('^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+\\-=_.]+$')
        ]
      ],
      password: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64),
          Validators.pattern(new RegExp(/(?=.*\d)(?=.*[a-z])/gm))
        ]
      ]
    });
  }

  async onSubmit(data: ILogin): Promise<void> {
    await this.loadingService.start('登入中...');
    this.accountService.login(data).subscribe(
      () => { this.loginSuccess(); },
      (err: HttpErrorResponse) => { this.loginFail(err); }
    );
  }

  async loginSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/']);
    await this.notificationService.toast('登入成功', 2000, SweetAlertIcon.success);
  }

  async loginFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.loginForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('登入失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const errors: IServerError = err.error;
        this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error).then();
        break;
      }
    }
  }

  nidUrl = (): string => this.accountService.nidLogin();

}
