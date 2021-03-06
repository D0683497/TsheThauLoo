import { Component, OnInit } from '@angular/core';
import { IManagerProfile } from '../../../../models/account/profile/manager/manager-profile.model';
import { BehaviorSubject } from 'rxjs';
import { ManagerService } from '../../../../services/account/manager/manager.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { LoadingService } from '../../../../services/loading/loading.service';
import { IDocument } from '../../../../models/document/document.model';
import { IServerError } from '../../../../models/error/server-error.model';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-jdenticon-sprites';

@Component({
  selector: 'app-manager-profile',
  templateUrl: './manager-profile.component.html',
  styleUrls: ['./manager-profile.component.scss'],
})
export class ManagerProfileComponent implements OnInit {

  date = Date.now();
  profile: IManagerProfile;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  showNationalId = false;
  photo: string;

  constructor(
    private managerService: ManagerService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private loadingService: LoadingService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.managerService.getProfile().subscribe(
      (res: IManagerProfile) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IManagerProfile): Promise<void> {
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
    await this.notificationService.message('????????????', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

  toggleNationalId = (): boolean => this.showNationalId = !this.showNationalId;

  async upload(event: any): Promise<void> {
    const files = event.files;
    if (files.length > 0) {
      const file = files[0];
      if (file.size > 2147483647) {
        await this.notificationService.message('????????????', SweetAlertIcon.info);
        return;
      }
      await this.loadingService.start('?????????...');
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
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
  }

  async uploadFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 403:
        const error: IServerError = err.error;
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      case 400:
        await this.notificationService.toast('????????????', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async delete(): Promise<void> {
    await this.loadingService.start('????????????');
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
    await this.notificationService.toast('????????????', 2000, SweetAlertIcon.success);
  }

  async deleteFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        await this.notificationService.toast('???????????????', 2000, SweetAlertIcon.error);
        break;
      case 400:
        await this.notificationService.toast('????????????', 2000, SweetAlertIcon.error);
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
        this.notificationService.toast('???????????????', 2000, SweetAlertIcon.error).then();
        break;
      case 400:
        this.notificationService.message('??????????????????', SweetAlertIcon.error).then();
        break;
    }
  }

}
