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
import { ICampaign } from '../../../models/activity/campaign/campaign.model';
import { CampaignService } from '../../../services/activity/campaign/campaign.service';

@Component({
  selector: 'app-campaign-display',
  templateUrl: './campaign-display.component.html',
  styleUrls: ['./campaign-display.component.scss'],
})
export class CampaignDisplayComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  campaign: ICampaign;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  type = RoleType;
  urlRoot = environment.apiUrl;

  constructor(
    private route: ActivatedRoute,
    private campaignService: CampaignService,
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
    this.campaignService.getCampaign(this.campaignId).subscribe(
      (res: ICampaign) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: ICampaign): Promise<void> {
    this.campaign = res;
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
      header: '系列活動附檔',
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
            const url = `https://docs.google.com/viewer?url=${this.urlRoot}/events/${this.campaignId}/files/${data.id}`;
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
    this.campaignService.getCampaignFile(this.campaignId, fileId).subscribe(
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

}
