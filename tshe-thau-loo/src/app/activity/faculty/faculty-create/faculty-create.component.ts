import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { OpeningService } from '../../../services/opening/opening.service';
import { IFaculty } from '../../../models/opening/faculty.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IFacultyCreate } from '../../../models/opening/faculty-create.model';

@Component({
  selector: 'app-faculty-create',
  templateUrl: './faculty-create.component.html',
  styleUrls: ['./faculty-create.component.scss'],
})
export class FacultyCreateComponent implements OnInit {

  @Input() campaignId: string;
  @Input() recruitmentId: string;
  @Input() openingId: string;
  date = Date.now();
  createForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private notificationService: NotificationService,
    private openingService: OpeningService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      description: [null, [Validators.required, Validators.maxLength(50)]]
    });
  }

  async onSubmit(data: IFacultyCreate): Promise<void> {
    await this.loadingService.start('修改中...');
    this.openingService.createFaculty(this.campaignId, this.recruitmentId, this.openingId, data).subscribe(
      (res: IFaculty) => { this.createSuccess(res); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  async createSuccess(res: IFaculty): Promise<void> {
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
          this.createForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
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
