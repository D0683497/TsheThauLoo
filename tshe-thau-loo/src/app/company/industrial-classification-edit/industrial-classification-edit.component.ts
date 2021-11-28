import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../services/loading/loading.service';
import { CompanyService } from '../../services/company/company.service';
import { NotificationService } from '../../services/notification/notification.service';
import { ICompany } from '../../models/company/company.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';
import { IServerError } from '../../models/error/server-error.model';
import { IIndustrialClassification } from '../../models/company/industrial-classification.model';
import { IIndustrialClassificationEdit } from '../../models/company/industrial-classification-edit.model';

@Component({
  selector: 'app-industrial-classification-edit',
  templateUrl: './industrial-classification-edit.component.html',
  styleUrls: ['./industrial-classification-edit.component.scss'],
})
export class IndustrialClassificationEditComponent implements OnInit {

  @Input() companyId: string;
  @Input() industrialClassification: IIndustrialClassification;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private companyService: CompanyService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      description: [this.industrialClassification.description, [Validators.required, Validators.maxLength(50)]]
    });
  }

  async onSubmit(data: IIndustrialClassificationEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.companyService.editSIC(this.companyId, this.industrialClassification.id, data).subscribe(
      (res: ICompany) => { this.createSuccess(res); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  async createSuccess(res: ICompany): Promise<void> {
    await this.modalController.dismiss(res);
    await this.loadingService.end();
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async createFail(err: HttpErrorResponse): Promise<void> {
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
