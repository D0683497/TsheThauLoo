import { Component, OnInit } from '@angular/core';
import { IAdministratorInfo } from '../../../../models/account/profile/administrator/administrator-info.model';
import { BehaviorSubject } from 'rxjs';
import { AdministratorService } from '../../../../services/account/administrator/administrator.service';
import { AccountService } from '../../../../services/account/account.service';
import { NotificationService } from '../../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../../enums/sweet-alert-icon.enum';
import { LoadingService } from '../../../../services/loading/loading.service';
import { IResponsibility } from '../../../../models/account/profile/administrator/responsibility.model';
import { ModalService } from '../../../../services/modal/modal.service';
import { IServerError } from '../../../../models/error/server-error.model';

@Component({
  selector: 'app-administrator-responsibility',
  templateUrl: './administrator-responsibility.component.html',
  styleUrls: ['./administrator-responsibility.component.scss'],
})
export class AdministratorResponsibilityComponent implements OnInit {

  date = Date.now();
  info: IAdministratorInfo;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);

  constructor(
    private administratorService: AdministratorService,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private loadingService: LoadingService,
    private modalService: ModalService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.administratorService.getInfo().subscribe(
      (res: IAdministratorInfo) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  getSuccess(res: IAdministratorInfo): void {
    this.info = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async create(): Promise<void> {
    const data = await this.modalService.createAdministratorResponsibility();
    if (data !== undefined) {
      this.info = data;
    }
  }

  async edit(responsibility: IResponsibility): Promise<void> {
    const data = await this.modalService.editAdministratorResponsibility(responsibility);
    if (data !== undefined) {
      this.info = data;
    }
  }

  async delete(responsibilityId: string): Promise<void> {
    await this.loadingService.start('刪除中...');
    this.administratorService.deleteResponsibility(responsibilityId).subscribe(
      (res: IAdministratorInfo) => { this.deleteSuccess(res); },
      (err: HttpErrorResponse) => { this.deleteFail(err); }
    );
  }

  async deleteSuccess(res: IAdministratorInfo): Promise<void> {
    this.info = res;
    await this.loadingService.end();
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 403: {
        const error: IServerError = err.error;
        await this.router.navigate(['/account/profile/administrator']);
        await this.notificationService.notify(error.title, error.detail, SweetAlertIcon.warning);
        break;
      }
      case 404:
      {
        await this.notificationService.message('查無此項目', SweetAlertIcon.question);
        break;
      }
      case 400:
      {
        await this.notificationService.message('刪除失敗', SweetAlertIcon.error);
        break;
      }
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
