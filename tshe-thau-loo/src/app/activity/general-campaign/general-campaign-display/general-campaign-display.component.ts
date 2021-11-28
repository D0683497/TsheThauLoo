import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RoleType } from '../../../enums/role-type.enum';
import { environment } from '../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { ModalService } from '../../../services/modal/modal.service';
import { ActionSheetController } from '@ionic/angular';
import { HttpErrorResponse } from '@angular/common/http';
import { IDocument } from '../../../models/document/document.model';
import { saveAs } from 'file-saver';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IGeneralCampaign } from '../../../models/activity/general-campaign/general-campaign.model';
import { GeneralCampaignService } from '../../../services/activity/general-campaign/general-campaign.service';
import { IServerError } from '../../../models/error/server-error.model';

@Component({
  selector: 'app-general-campaign-display',
  templateUrl: './general-campaign-display.component.html',
  styleUrls: ['./general-campaign-display.component.scss'],
})
export class GeneralCampaignDisplayComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  generalId = this.route.snapshot.paramMap.get('generalId');
  generalCampaign: IGeneralCampaign;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  type = RoleType;
  canAttendee = false;
  urlRoot = environment.apiUrl;

  constructor(
    private route: ActivatedRoute,
    private generalCampaignService: GeneralCampaignService,
    public authService: AuthService,
    private loadingService: LoadingService,
    private notificationService: NotificationService,
    private modalService: ModalService,
    private actionSheetController: ActionSheetController) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.generalCampaignService.getGeneralCampaign(this.campaignId, this.generalId).subscribe(
      (res: IGeneralCampaign) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IGeneralCampaign): Promise<void> {
    this.generalCampaign = res;
    if (res.registrationStartTime !== null && res.registrationEndTime !== null) {
      if (new Date(res.registrationStartTime).getTime() < this.date && new Date(res.registrationEndTime).getTime() > this.date) {
        this.canAttendee = true;
      }
    } else {
      if (new Date(res.startTime).getTime() > this.date) {
        this.canAttendee = true;
      }
    }
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async option(data: IDocument): Promise<void> {
    const options = {
      translucent: true,
      header: '一般子活動附檔',
      buttons: [
        {
          text: '下載',
          handler: () => {
            this.download(data.id, data.name+data.extension);
          }
        },
        {
          text: '預覽',
          handler: () => {
            const url = `https://docs.google.com/viewer?url=
                          ${this.urlRoot}/campaigns/${this.campaignId}/general/${this.generalId}/files/${data.id}`;
            window.open(url, '_blank');
          }
        },
        {
          text: '取消',
          icon: 'close',
          role: 'cancel',
        }
      ]
    };
    const actionSheet = await this.actionSheetController.create(options);
    await actionSheet.present();
  }

  async download(fileId: string, fileName: string): Promise<void> {
    await this.loadingService.start('下載中...');
    this.generalCampaignService.getGeneralCampaignFile(this.campaignId, this.generalId, fileId).subscribe(
      (res: Blob) => { this.downloadSuccess(res, fileName); },
      (err: HttpErrorResponse) => { this.downloadFail(err); }
    );
  }

  async downloadSuccess(res: Blob, fileName: string): Promise<void> {
    await this.loadingService.end();
    saveAs(res, fileName);
    await this.notificationService.toast('下載成功', 2000, SweetAlertIcon.success);
  }

  async downloadFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        await this.notificationService.toast('查無此檔案', 2000, SweetAlertIcon.error);
        break;
      case 400:
        await this.notificationService.message('發生未知錯誤', SweetAlertIcon.error);
        break;
    }
  }

  async showDeclaration(): Promise<boolean> {
    if (this.generalCampaign.declaration !== null) {
      return await this.modalService.activityDeclaration(this.generalCampaign.declaration);
    } else {
      return true;
    }
  }

  async attendee(): Promise<void> {
    if (await this.showDeclaration()) {
      await this.loadingService.start('報名中...');
      this.generalCampaignService.signUpGeneralCampaign(this.campaignId, this.generalId).subscribe(
        () => { this.attendeeSuccess(); },
        (err: HttpErrorResponse) => { this.attendeeFail(err); }
      );
    }
  }

  async attendeeSuccess(): Promise<void> {
    await this.loadingService.end();
    if (this.generalCampaign.enableVerify) {
      await this.notificationService.notify('已送出報名', '請等待管理員審核', SweetAlertIcon.success);
    } else {
      await this.notificationService.message('報名成功', SweetAlertIcon.success);
    }
  }

  async attendeeFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 403) {
      const errors: IServerError = err.error;
      await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
    }
  }

}
