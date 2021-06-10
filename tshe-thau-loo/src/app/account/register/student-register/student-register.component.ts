import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalService } from '../../../services/modal/modal.service';
import { StudentService } from '../../../services/account/student/student.service';
import { Router } from '@angular/router';
import { NotificationService } from '../../../services/notification/notification.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { PhoneNumberValidator } from '../../../validators/phone.validator';
import { NationalIdValidator } from '../../../validators/taiwan-id.validator';
import { MustMatch } from '../../../validators/must-match.validator';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IStudentRegister } from '../../../models/account/register/student-register.model';

@Component({
  selector: 'app-student-register',
  templateUrl: './student-register.component.html',
  styleUrls: ['./student-register.component.scss'],
})
export class StudentRegisterComponent implements OnInit {

  date = new Date().toISOString();
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private modalService: ModalService,
    private studentService: StudentService,
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
      // 學生資料
      networkId: [null, [Validators.required, Validators.maxLength(10)]],
      college: [null, [Validators.required, Validators.maxLength(20)]],
      department: [null, [Validators.required, Validators.maxLength(20)]],
      class: [null, [Validators.required, Validators.maxLength(20)]]
    },{ validators: MustMatch('password', 'passwordConfirm') });
  }

  async onSubmit(data: IStudentRegister): Promise<void> {
    if (await this.modalService.registerTerms()) {
      await this.loadingService.start('註冊中...');
      this.studentService.register(data).subscribe(
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
