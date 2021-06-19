import { Component, Input, OnInit } from '@angular/core';
import { IDocument } from '../../../models/document/document.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../../services/loading/loading.service';
import { StudentService } from '../../../services/account/student/student.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { IDocumentEdit } from '../../../models/document/document-edit.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { RoleType } from '../../../enums/role-type.enum';
import { AlumnusService } from '../../../services/account/alumnus/alumnus.service';
import { AccountService } from '../../../services/account/account.service';

@Component({
  selector: 'app-edit-verify-file',
  templateUrl: './edit-verify-file.component.html',
  styleUrls: ['./edit-verify-file.component.scss'],
})
export class EditVerifyFileComponent implements OnInit {

  @Input() role: RoleType;
  @Input() file: IDocument;
  date = Date.now();
  editForm: FormGroup;

  constructor(
    private modalController: ModalController,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private alumnusService: AlumnusService,
    private studentService: StudentService,
    private notificationService: NotificationService,
    private accountService: AccountService) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      name: [this.file.name, [Validators.required, Validators.maxLength(260)]],
      extension: [this.file.extension, [Validators.maxLength(10)]],
    });
  }

  async onSubmit(data: IDocumentEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    switch (this.role) {
      case RoleType.alumnus:
        this.alumnusService.editVerifyFile(this.file.id, data).subscribe(
          (res: IDocument) => { this.editSuccess(res); },
          (err: HttpErrorResponse) => { this.editFail(err); }
        );
        break;
      case RoleType.student:
        this.studentService.editVerifyFile(this.file.id, data).subscribe(
          (res: IDocument) => { this.editSuccess(res); },
          (err: HttpErrorResponse) => { this.editFail(err); }
        );
        break;
      default:
        this.accountService.editNationalVerifyFile(this.file.id, data).subscribe(
          (res: IDocument) => { this.editSuccess(res); },
          (err: HttpErrorResponse) => { this.editFail(err); }
        );
        break;
    }
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
