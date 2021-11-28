import { Component, Input, OnInit } from '@angular/core';
import { IDocument } from '../../models/document/document.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../services/loading/loading.service';
import { ResumeService } from '../../services/resume/resume.service';
import { NotificationService } from '../../services/notification/notification.service';
import { IDocumentEdit } from '../../models/document/document-edit.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../models/error/form-error.model';

@Component({
  selector: 'app-resume-file-edit',
  templateUrl: './resume-file-edit.component.html',
  styleUrls: ['./resume-file-edit.component.scss'],
})
export class ResumeFileEditComponent implements OnInit {

  @Input() resume: IDocument;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private resumeService: ResumeService,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.editForm = this.fb.group({
      name: [this.resume.name, [Validators.required, Validators.maxLength(260)]],
      extension: [this.resume.extension, [Validators.maxLength(10)]]
    });
  }

  async onSubmit(data: IDocumentEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.resumeService.editResume(this.resume.id, data).subscribe(
      (res: IDocument) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IDocument): Promise<void> {
    await this.loadingService.end();
    await this.modalController.dismiss(res);
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('修改失敗', SweetAlertIcon.error);
    }
  }

  async dismiss(): Promise<void> {
    await this.modalController.dismiss();
  }

}
