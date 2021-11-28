import { Component, Input, OnInit } from '@angular/core';
import { IResponsibility } from '../../../../models/account/profile/administrator/responsibility.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../../services/loading/loading.service';
import { AdministratorService } from '../../../../services/account/administrator/administrator.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { IAdministratorInfo } from '../../../../models/account/profile/administrator/administrator-info.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { IResponsibilityEdit } from '../../../../models/account/profile/administrator/responsibility-edit.model';

@Component({
  selector: 'app-administrator-edit-responsibility',
  templateUrl: './administrator-edit-responsibility.component.html',
  styleUrls: ['./administrator-edit-responsibility.component.scss'],
})
export class AdministratorEditResponsibilityComponent implements OnInit {

  @Input() responsibility: IResponsibility;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private administratorService: AdministratorService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      description: [this.responsibility.description, [Validators.required, Validators.maxLength(200)]]
    });
  }

  async onSubmit(data: IResponsibilityEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.administratorService.editResponsibility(this.responsibility.id, data).subscribe(
      (res: IAdministratorInfo) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAdministratorInfo): Promise<void> {
    await this.modalController.dismiss(res);
    await this.loadingService.end();
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
