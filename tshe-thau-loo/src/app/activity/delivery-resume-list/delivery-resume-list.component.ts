import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActionSheetController, ModalController } from '@ionic/angular';
import { HttpErrorResponse } from '@angular/common/http';
import { IDeliveryResume } from '../../models/activity/recruitment-campaign/delivery-resume.model';
import { OpeningService } from '../../services/opening/opening.service';
import { IDocument } from '../../models/document/document.model';
import { saveAs } from 'file-saver';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { LoadingService } from '../../services/loading/loading.service';
import { NotificationService } from '../../services/notification/notification.service';

@Component({
  selector: 'app-delivery-resume-list',
  templateUrl: './delivery-resume-list.component.html',
  styleUrls: ['./delivery-resume-list.component.scss'],
})
export class DeliveryResumeListComponent implements OnInit {

  @Input() campaignId: string;
  @Input() recruitmentId: string;
  @Input() openingId: string;
  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  resumes: IDeliveryResume[];

  constructor(
    private modalController: ModalController,
    private openingService: OpeningService,
    private actionSheetController: ActionSheetController,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  async ngOnInit(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.openingService.deliveryResumeList(this.campaignId, this.recruitmentId, this.openingId).subscribe(
      (res: IDeliveryResume[]) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IDeliveryResume[]): Promise<void> {
    this.resumes = res;
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
      header: '履歷',
      buttons: [
        {
          text: '下載',
          handler: () => {
            this.download(data.id, data.name+data.extension);
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
    this.openingService.deliveryResumeDownload(this.campaignId, this.recruitmentId, this.openingId, fileId).subscribe(
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

  async dismiss(): Promise<void> {
    await this.modalController.dismiss();
  }

}
