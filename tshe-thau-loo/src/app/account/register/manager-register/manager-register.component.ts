import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalService } from '../../../services/modal/modal.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/notification/notification.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { ManagerService } from '../../../services/account/manager/manager.service';
import { PhoneNumberValidator } from '../../../validators/phone.validator';
import { NationalIdValidator } from '../../../validators/taiwan-id.validator';
import { MustMatch } from '../../../validators/must-match.validator';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IManagerRegister } from '../../../models/account/register/manager-register.model';

@Component({
  selector: 'app-manager-register',
  templateUrl: './manager-register.component.html',
  styleUrls: ['./manager-register.component.scss'],
})
export class ManagerRegisterComponent implements OnInit {

  date = new Date().toISOString();
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private modalService: ModalService,
    private managerService: ManagerService,
    private router: Router,
    private notificationService: NotificationService,
    private loadingService: LoadingService) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      // 基本資料
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
      ],
      passwordConfirm: [
        null,
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(64)
        ]
      ],
      email: [null, [Validators.required, Validators.email, Validators.maxLength(320)]],
      phoneNumber: [null, [Validators.maxLength(30), PhoneNumberValidator('TW')]],
      // 個人資料
      nationalId: [null, [Validators.maxLength(10), NationalIdValidator()]],
      name: [null, [Validators.required, Validators.maxLength(50)]],
      gender: [null],
      dateOfBirth: [null],
      currentAddress: [null, [Validators.maxLength(200)]],
      // 企業使用者資料
      divisionName: [null, [Validators.required, Validators.maxLength(30)]],
      jobTitle: [null, [Validators.required, Validators.maxLength(30)]],
      contactEmail: [null, [Validators.required, Validators.email, Validators.maxLength(320)]],
      contactPhone: [
        null,
        [
          Validators.required,
          Validators.maxLength(30),
          PhoneNumberValidator('TW')
        ]
      ],
      contactAddress: [null, [Validators.required, Validators.maxLength(200)]],
      // 職務代理人資料
      substitute: this.fb.group({
        name: [null, [Validators.required, Validators.maxLength(50)]],
        divisionName: [null, [Validators.required, Validators.maxLength(30)]],
        jobTitle: [null, [Validators.required, Validators.maxLength(30)]],
        contactEmail: [null, [Validators.required, Validators.email, Validators.maxLength(320)]],
        contactPhone: [
          null,
          [
            Validators.required,
            Validators.maxLength(30),
            PhoneNumberValidator('TW')
          ]
        ],
        contactAddress: [null, [Validators.required, Validators.maxLength(200)]]
      })
    },{ validators: MustMatch('password', 'passwordConfirm') });
  }

  async onSubmit(data: IManagerRegister): Promise<void> {
    console.log(data);
    if (await this.modalService.registerTerms()) {
      await this.loadingService.start('註冊中...');
      this.managerService.register(data).subscribe(
        () => { this.registerSuccess(); },
        (err: HttpErrorResponse) => { this.registerFail(err); }
      );
    }
  }

  async registerSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.notificationService.notify('註冊成功', '請前往您的信箱收取驗證信', SweetAlertIcon.success);
    await this.router.navigate(['/account/login']);
  }

  async registerFail(err: HttpErrorResponse): Promise<void> {
    console.log(err);
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.registerForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('註冊失敗', SweetAlertIcon.error);
    }
  }

}
