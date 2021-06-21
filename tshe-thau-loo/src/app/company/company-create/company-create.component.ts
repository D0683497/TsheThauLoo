import { Component, OnInit } from '@angular/core';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrationNumberValidator } from '../../validators/taiwan-gui.validator';
import { HttpErrorResponse } from '@angular/common/http';
import { ICompanyCreate } from '../../models/company/company-create.model';
import { IFormError } from '../../models/error/form-error.model';
import { LoadingService } from '../../services/loading/loading.service';
import { CompanyService } from '../../services/company/company.service';
import { ICompany } from '../../models/company/company.model';
import { IServerError } from '../../models/error/server-error.model';

@Component({
  selector: 'app-company-create',
  templateUrl: './company-create.component.html',
  styleUrls: ['./company-create.component.scss'],
})
export class CompanyCreateComponent implements OnInit {

  date = Date.now();
  createForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private companyService: CompanyService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      registrationNumber: [null, [Validators.required, Validators.maxLength(10), RegistrationNumberValidator()]],
      name: [null, [Validators.required, Validators.maxLength(100)]],
      introduction: [null],
      website: [
        null,
        [
          Validators.maxLength(300),
          // eslint-disable-next-line max-len
          Validators.pattern(new RegExp('https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_+.~#?&/=]*)'))
        ]
      ]
    });
  }

  async onSubmit(data: ICompanyCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.companyService.create(data).subscribe(
      (res: ICompany) => { this.registerSuccess(res); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  async registerSuccess(res: ICompany): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate([`/company/${res.id}/edit`]);
    await this.notificationService.notify('建立成功', '即將前往公司編輯頁面', SweetAlertIcon.success);
  }

  async registerFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.createForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('註冊失敗', SweetAlertIcon.error);
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

}
