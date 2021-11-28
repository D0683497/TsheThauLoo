import { Component, Input, OnInit } from '@angular/core';
import { IQualification } from '../../../models/opening/qualification.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { OpeningService } from '../../../services/opening/opening.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IQualificationEdit } from '../../../models/opening/qualification-edit.model';

@Component({
  selector: 'app-qualification-edit',
  templateUrl: './qualification-edit.component.html',
  styleUrls: ['./qualification-edit.component.scss'],
})
export class QualificationEditComponent implements OnInit {

  @Input() campaignId: string;
  @Input() recruitmentId: string;
  @Input() openingId: string;
  @Input() qualification: IQualification;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private notificationService: NotificationService,
    private openingService: OpeningService) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      description: [this.qualification.description, [Validators.required, Validators.maxLength(200)]]
    });
  }

  async onSubmit(data: IQualificationEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.openingService.editQualification(this.campaignId, this.recruitmentId, this.openingId, this.qualification.id, data).subscribe(
      (res: IQualification) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IQualification): Promise<void> {
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
