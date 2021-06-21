import { Component, OnInit } from '@angular/core';
import { ICompany } from '../../models/company/company.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyService } from '../../services/company/company.service';
import { HttpErrorResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-identicon-sprites';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { RegistrationNumberValidator } from '../../validators/taiwan-gui.validator';
import { ICompanyEdit } from '../../models/company/company-edit.model';
import { LoadingService } from '../../services/loading/loading.service';
import { IFormError } from '../../models/error/form-error.model';
import { IServerError } from '../../models/error/server-error.model';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.scss'],
})
export class CompanyEditComponent implements OnInit {

  date = Date.now();
  companyId = this.route.snapshot.paramMap.get('companyId');
  company: ICompany;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  photo: string;
  segment = 'info';

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private companyService: CompanyService,
    private route: ActivatedRoute,
    private loadingService: LoadingService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.companyService.getCompany(this.companyId).subscribe(
      (res: ICompany) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: ICompany): void {
    this.company = res;
    this.buildForm(res);
    if (res.hasLogo) {
      // await this.downloadPhoto();
    } else {
      this.photo = createAvatar(style, {
        seed: res.id,
        dataUri: true
      });
      this.loading$.next(false);
      this.loadingError$.next(false);
    }
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: ICompany): void {
    this.editForm = this.fb.group({
      registrationNumber: [data.registrationNumber, [Validators.required, Validators.maxLength(10), RegistrationNumberValidator()]],
      name: [data.name, [Validators.required, Validators.maxLength(100)]],
      introduction: [data.introduction],
      website: [
        data.website,
        [
          Validators.maxLength(300),
          // eslint-disable-next-line max-len
          Validators.pattern(new RegExp('https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_+.~#?&/=]*)'))
        ]
      ]
    });
  }

  async onSubmit(data: ICompanyEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.companyService.edit(this.companyId, data).subscribe(
      (res: ICompany) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: ICompany): Promise<void> {
    await this.loadingService.end();
    this.company = res;
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
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
        await this.notificationService.message('修改失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const errors: IServerError = err.error;
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
      }
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  segmentChanged = (ev: CustomEvent): void =>this.segment = ev.detail.value;

}
