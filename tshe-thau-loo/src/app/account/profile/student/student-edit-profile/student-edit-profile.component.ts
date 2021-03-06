import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { IStudentInfo } from '../../../../models/account/profile/student/student-info.model';
import { StudentService } from '../../../../services/account/student/student.service';
import { IStudentEditInfo } from '../../../../models/account/profile/student/student-edit-info.model';

@Component({
  selector: 'app-student-edit-profile',
  templateUrl: './student-edit-profile.component.html',
  styleUrls: ['./student-edit-profile.component.scss'],
})
export class StudentEditProfileComponent implements OnInit {

  date = Date.now();
  info: IStudentInfo;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private studentService: StudentService,
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
    this.studentService.getInfo().subscribe(
      (res: IStudentInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IStudentInfo): void {
    this.info = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IStudentInfo): void {
    this.editForm = this.fb.group({
      networkId: [data.networkId, [Validators.required, Validators.maxLength(10)]],
      college: [data.college, [Validators.required, Validators.maxLength(20)]],
      department: [data.department, [Validators.required, Validators.maxLength(20)]],
      class: [data.class, [Validators.required, Validators.maxLength(20)]]
    });
  }

  async onSubmit(data: IStudentEditInfo): Promise<void> {
    await this.loadingService.start('?????????...');
    this.studentService.editInfo(data).subscribe(
      (res: IStudentInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IStudentInfo): Promise<void> {
    await this.loadingService.end();
    this.info = res;
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('????????????', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/student']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('????????????', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
