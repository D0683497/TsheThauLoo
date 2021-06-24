import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';
import { IServerError } from '../../models/error/server-error.model';
import { NotificationService } from '../../services/notification/notification.service';
import { CompanyService } from '../../services/company/company.service';
import { IIndustrialClassificationCreate } from '../../models/company/industrial-classification-create.model';
import { ICompany } from '../../models/company/company.model';

@Component({
  selector: 'app-industrial-classification-create',
  templateUrl: './industrial-classification-create.component.html',
  styleUrls: ['./industrial-classification-create.component.scss'],
})
export class IndustrialClassificationCreateComponent implements OnInit {

  @Input() companyId: string;
  date = Date.now();
  createForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private companyService: CompanyService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      description: [null, [Validators.required, Validators.maxLength(50)]]
    });
  }

  async onSubmit(data: IIndustrialClassificationCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.companyService.createSIC(this.companyId, data).subscribe(
      (res: ICompany) => { this.createSuccess(res); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  async createSuccess(res: ICompany): Promise<void> {
    await this.modalController.dismiss(res);
    await this.loadingService.end();
    await this.notificationService.toast('建立成功', 2000, SweetAlertIcon.success);
  }

  async createFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.createForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('建立失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.modalController.dismiss();
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async dismiss(): Promise<void> {
    await this.modalController.dismiss();
  }

}
