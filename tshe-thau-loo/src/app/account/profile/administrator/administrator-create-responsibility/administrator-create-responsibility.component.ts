import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { IAdministratorInfo } from '../../../../models/account/profile/administrator/administrator-info.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { LoadingService } from '../../../../services/loading/loading.service';
import { AdministratorService } from '../../../../services/account/administrator/administrator.service';
import { IResponsibilityCreate } from '../../../../models/account/profile/administrator/responsibility-create.model';
import { NotificationService } from '../../../../services/notification/notification.service';

@Component({
  selector: 'app-administrator-create-responsibility',
  templateUrl: './administrator-create-responsibility.component.html',
  styleUrls: ['./administrator-create-responsibility.component.scss'],
})
export class AdministratorCreateResponsibilityComponent implements OnInit {

  date = Date.now();
  createForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private administratorService: AdministratorService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      description: [null, [Validators.required, Validators.maxLength(200)]]
    });
  }

  async onSubmit(data: IResponsibilityCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.administratorService.createResponsibility(data).subscribe(
      (res: IAdministratorInfo) => { this.createSuccess(res); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  async createSuccess(res: IAdministratorInfo): Promise<void> {
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
