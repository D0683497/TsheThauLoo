import { Component, OnInit } from '@angular/core';
import { IAlumnusProfile } from '../../../../models/account/profile/alumnus/alumnus-profile.model';
import { BehaviorSubject } from 'rxjs';
import { AlumnusService } from '../../../../services/account/alumnus/alumnus.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { LoadingService } from '../../../../services/loading/loading.service';
import { IServerError } from '../../../../models/error/server-error.model';
import { IDocument } from '../../../../models/document/document.model';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';

@Component({
  selector: 'app-alumnus-profile',
  templateUrl: './alumnus-profile.component.html',
  styleUrls: ['./alumnus-profile.component.scss'],
})
export class AlumnusProfileComponent implements OnInit {

  date = Date.now();
  profile: IAlumnusProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;
  photo: string;

  constructor(
    private alumnusService: AlumnusService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private loadingService: LoadingService) { }

  ngOnInit() {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.alumnusService.getProfile().subscribe(
      (res: IAlumnusProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IAlumnusProfile): Promise<void> {
    this.profile = res;
    if (res.hasPhoto) {
      await this.downloadPhoto();
    } else {
      this.photo = createAvatar(style, {
        seed: res.id,
        dataUri: true
      });
      this.loading$.next(false);
      this.loadingError$.next(false);
    }
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  toggleNationalId = (): boolean => this.showNationalId = !this.showNationalId;

  async upload(event: any): Promise<void> {
    const files = event.files;
    if (files.length > 0) {
      const file = files[0];
      if (file.size > 2147483647) {
        await this.notificationService.message('檔案過大', SweetAlertIcon.info);
        return;
      }
      await this.loadingService.start('上傳中...');
      this.accountService.createPhoto(file).subscribe(
        (res: IDocument) => { this.uploadSuccess(file); },
        (err: HttpErrorResponse) => { this.uploadFail(err); }
      );
    }
  }

  async uploadSuccess(file: Blob): Promise<void> {
    this.profile.hasPhoto = true;
    this.photo = URL.createObjectURL(file);
    await this.loadingService.end();
    await this.notificationService.toast('上傳成功', 2000, SweetAlertIcon.success);
  }

  async uploadFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 403:
        const error: IServerError = err.error;
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      case 400:
        await this.notificationService.toast('上傳失敗', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async delete(): Promise<void> {
    await this.loadingService.start('刪除中…');
    this.accountService.deletePhoto().subscribe(
      () => { this.deleteSuccess(); },
      (err: HttpErrorResponse) => { this.deleteFail(err); }
    );
  }

  async deleteSuccess(): Promise<void> {
    this.profile.hasPhoto = false;
    this.photo = createAvatar(style, {
      seed: this.profile.id,
      dataUri: true
    });
    await this.loadingService.end();
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        await this.notificationService.toast('查無此檔案', 2000, SweetAlertIcon.error);
        break;
      case 400:
        await this.notificationService.toast('刪除失敗', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async downloadPhoto(): Promise<void> {
    this.accountService.getPhoto().subscribe(
      (res: Blob) => { this.downloadSuccess(res); },
      (err: HttpErrorResponse) => { this.downloadFail(err); }
    );
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async downloadSuccess(res: Blob): Promise<void> {
    this.photo = URL.createObjectURL(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async downloadFail(err: HttpErrorResponse): Promise<void> {
    this.loading$.next(false);
    this.loadingError$.next(true);
    switch (err.status) {
      case 404:
        this.notificationService.toast('查無此檔案', 2000, SweetAlertIcon.error).then();
        break;
      case 400:
        this.notificationService.message('發生未知錯誤', SweetAlertIcon.error).then();
        break;
    }
  }

}
