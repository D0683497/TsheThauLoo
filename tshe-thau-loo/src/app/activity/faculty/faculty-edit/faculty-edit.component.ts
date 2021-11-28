import { Component, Input, OnInit } from '@angular/core';
import { IFaculty } from '../../../models/opening/faculty.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { OpeningService } from '../../../services/opening/opening.service';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IFacultyEdit } from '../../../models/opening/faculty-edit.model';

@Component({
  selector: 'app-faculty-edit',
  templateUrl: './faculty-edit.component.html',
  styleUrls: ['./faculty-edit.component.scss'],
})
export class FacultyEditComponent implements OnInit {

  @Input() campaignId: string;
  @Input() recruitmentId: string;
  @Input() openingId: string;
  @Input() faculty: IFaculty;
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
      description: [this.faculty.description, [Validators.required, Validators.maxLength(50)]]
    });
  }

  async onSubmit(data: IFacultyEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.openingService.editFaculty(this.campaignId, this.recruitmentId, this.openingId, this.faculty.id, data).subscribe(
      (res: IFaculty) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IFaculty): Promise<void> {
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
