import { Component, Input, OnInit } from '@angular/core';
import { IDocument } from '../../../../models/document/document.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../../services/loading/loading.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { IDocumentEdit } from '../../../../models/document/document-edit.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { AlumnusService } from '../../../../services/account/alumnus/alumnus.service';

@Component({
  selector: 'app-alumnus-edit-verify-file',
  templateUrl: './alumnus-edit-verify-file.component.html',
  styleUrls: ['./alumnus-edit-verify-file.component.scss'],
})
export class AlumnusEditVerifyFileComponent implements OnInit {

  @Input() file: IDocument;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private alumnusService: AlumnusService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      name: [this.file.name, [Validators.required, Validators.maxLength(260)]],
      extension: [this.file.extension, [Validators.maxLength(10)]],
    });
  }

  async onSubmit(data: IDocumentEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.alumnusService.editVerifyFile(this.file.id, data).subscribe(
      (res: IDocument) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IDocument): Promise<void> {
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
