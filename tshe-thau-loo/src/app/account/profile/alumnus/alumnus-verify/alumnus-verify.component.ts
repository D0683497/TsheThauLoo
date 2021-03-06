import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../../services/loading/loading.service';
import { ModalService } from '../../../../services/modal/modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../../models/error/form-error.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { IDocument } from '../../../../models/document/document.model';
import { saveAs } from 'file-saver';
import { IAlumnusVerify } from '../../../../models/account/profile/alumnus/alumnus-verify.model';
import { AlumnusService } from '../../../../services/account/alumnus/alumnus.service';
import { IAlumnusEditVerify } from '../../../../models/account/profile/alumnus/alumnus-edit-verify.model';
import { RoleType } from '../../../../enums/role-type.enum';

@Component({
  selector: 'app-alumnus-verify',
  templateUrl: './alumnus-verify.component.html',
  styleUrls: ['./alumnus-verify.component.scss'],
})
export class AlumnusVerifyComponent implements OnInit {

  date = Date.now();
  segment = 'info';
  verify: IAlumnusVerify;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private alumnusService: AlumnusService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private modalService: ModalService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.alumnusService.getVerify().subscribe(
      (res: IAlumnusVerify) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAlumnusVerify): void {
    this.verify = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IAlumnusVerify): void {
    this.editForm = this.fb.group({
      description: [data.description, Validators.maxLength(500)]
    });
  }

  async onSubmit(data: IAlumnusEditVerify): Promise<void> {
    await this.loadingService.start('?????????...');
    this.alumnusService.editVerify(data).subscribe(
      (res: IAlumnusVerify) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IAlumnusVerify): Promise<void> {
    await this.loadingService.end();
    this.verify = res;
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
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
        await this.notificationService.message('????????????', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/alumnus']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
    }
  }

  async upload(event: any): Promise<void> {
    const files = event.files;
    if (files.length > 0) {
      const file = files[0];
      if (file.size > 2147483647) {
        await this.notificationService.message('????????????', SweetAlertIcon.info);
        return;
      }
      await this.loadingService.start('?????????...');
      this.alumnusService.createVerifyFile(file).subscribe(
        (res: IDocument) => { this.uploadSuccess(res); },
        (err: HttpErrorResponse) => { this.uploadFail(err); }
      );
    }
  }

  async uploadSuccess(res: IDocument): Promise<void> {
    this.verify.files.push(res);
    await this.loadingService.end();
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
  }

  async uploadFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 403:
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/alumnus']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      case 400:
        await this.notificationService.toast('????????????', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async delete(fileId: string): Promise<void> {
    await this.loadingService.start('????????????');
    this.alumnusService.deleteVerifyFile(fileId).subscribe(
      () => { this.deleteSuccess(fileId); },
      (err: HttpErrorResponse) => { this.deleteFail(err); }
    );
  }

  async deleteSuccess(fileId: string): Promise<void> {
    const index = this.verify.files.findIndex(x => x.id === fileId);
    this.verify.files.splice(index, 1);
    await this.loadingService.end();
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
  }

  async deleteFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 403:
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/alumnus']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      case 404:
        await this.notificationService.toast('???????????????', 2000, SweetAlertIcon.error);
        break;
      case 400:
        await this.notificationService.toast('????????????', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async download(fileId: string, fileName: string): Promise<void> {
    await this.loadingService.start('????????????');
    this.alumnusService.downloadVerifyFile(fileId).subscribe(
      (res: Blob) => { this.downloadSuccess(res, fileName); },
      (err: HttpErrorResponse) => { this.downloadFail(err); }
    );
  }

  async downloadSuccess(res: Blob, fileName: string): Promise<void> {
    saveAs(res, fileName);
    await this.loadingService.end();
  }

  async downloadFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        this.notificationService.toast('???????????????', 2000, SweetAlertIcon.error).then();
        break;
      case 400:
        this.notificationService.message('??????????????????', SweetAlertIcon.error).then();
        break;
    }
  }

  async editFile(file: IDocument): Promise<void> {
    const data = await this.modalService.editVerifyFile(RoleType.alumnus, file);
    if (data !== undefined) {
      const index = this.verify.files.findIndex(x => x.id === data.id);
      this.verify.files[index] = data;
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('????????????', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  segmentChanged = (ev: any): void =>this.segment = ev.detail.value;

}
