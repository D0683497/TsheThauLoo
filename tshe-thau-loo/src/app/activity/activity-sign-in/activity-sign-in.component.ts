import { Component, Input, OnInit } from '@angular/core';
import { ActivityType } from '../../enums/activity-type.enum';
import { ModalController } from '@ionic/angular';
import { LoadingService } from '../../services/loading/loading.service';
import { NotificationService } from '../../services/notification/notification.service';
import { EventService } from '../../services/activity/event/event.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { ActivityActionType } from '../../enums/activity-action-type.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { IServerError } from '../../models/error/server-error.model';
import { IActivityAttendeeSignIn } from '../../models/activity/activity-attendee-sign-in.model';
import { IActivityParticipantSignIn } from '../../models/activity/activity-participant-sign-in.model';
import { GeneralCampaignService } from '../../services/activity/general-campaign/general-campaign.service';

@Component({
  selector: 'app-activity-sign-in',
  templateUrl: './activity-sign-in.component.html',
  styleUrls: ['./activity-sign-in.component.scss'],
})
export class ActivitySignInComponent implements OnInit {

  @Input() firstId: string;
  @Input() secondId: string;
  @Input() type: ActivityType;
  date = Date.now();
  hasDevices: boolean;
  hasPermission: boolean;
  availableDevices: MediaDeviceInfo[]; //可用的裝置
  deviceSelected: string; //選擇的裝置
  deviceCurrent: MediaDeviceInfo; //選擇的裝置

  constructor(
    private modalController: ModalController,
    private loadingService: LoadingService,
    private notificationService: NotificationService,
    private eventService: EventService,
    private generalCampaignService: GeneralCampaignService) { }

  ngOnInit(): void {}

  async dismiss(): Promise<void> {
    await this.modalController.dismiss();
  }

  scanSuccess(resultString: string): void {
    this.loadingService.start('請稍後...').then();
    const data = JSON.parse(resultString);
    if (!this.checkQRCode(data)) {
      return;
    }
    try {
      switch (data.action) {
        case ActivityActionType.attendee:
          this.attendee(data);
          break;
        case ActivityActionType.participant:
          this.participant(data);
          break;
      }
    } catch (e) {
      this.loadingService.end().then();
      this.notificationService.message('無法解析 QRCode', SweetAlertIcon.error).then();
    }
  }

  checkQRCode(data: any): boolean {
    try {
      if (this.firstId !== data.firstId || this.secondId !== data.secondId || this.type !== data.type) {
        this.loadingService.end().then();
        this.notificationService.message('QRCode 錯誤', SweetAlertIcon.error).then();
        return false;
      }
    } catch (e) {
      this.loadingService.end().then();
      this.notificationService.message('無法解析 QRCode', SweetAlertIcon.error).then();
      return false;
    }
    return true;
  }

  attendee(data: IActivityAttendeeSignIn): void {
    try {
      switch (this.type) {
        case ActivityType.event:
          this.eventService.signInEvent(data.firstId, data.userId).subscribe(
            () => { this.signInSuccess(); },
            (err: HttpErrorResponse) => { this.signInFail(err); }
          );
          break;
        case ActivityType.generalCampaign:
          this.generalCampaignService.signInGeneralCampaign(data.firstId, data.secondId, data.userId).subscribe(
            () => { this.signInSuccess(); },
            (err: HttpErrorResponse) => { this.signInFail(err); }
          );
          break;
      }
    } catch (e) {
      this.loadingService.end().then();
      this.notificationService.message('無法解析 QRCode', SweetAlertIcon.error).then();
    }
  }

  participant(data: IActivityParticipantSignIn): void {
    try {
      switch (this.type) {
        case ActivityType.event:
          this.eventService.participateEvent(data.firstId,
            {name: data.name, contactPhone: data.contactPhone, remark: data.remark}).subscribe(
            () => { this.signInSuccess(); },
            (err: HttpErrorResponse) => { this.signInFail(err); }
          );
          break;
        case ActivityType.generalCampaign:
          this.generalCampaignService.participateGeneralCampaign(data.firstId, data.secondId,
            {name: data.name, contactPhone: data.contactPhone, remark: data.remark}).subscribe(
            () => { this.signInSuccess(); },
            (err: HttpErrorResponse) => { this.signInFail(err); }
          );
          break;
      }
    } catch (e) {
      this.loadingService.end().then();
      this.notificationService.message('無法解析 QRCode', SweetAlertIcon.error).then();
    }
  }

  signInSuccess(): void {
    this.loadingService.end().then();
    this.notificationService.toast('簽到成功', 2000, SweetAlertIcon.success).then();
  }

  signInFail(err: HttpErrorResponse): void {
    this.loadingService.end().then();
    switch (err.status) {
      case 403:
      {
        const errors: IServerError = err.error;
        this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error).then();
        break;
      }
      case 400:
      {
        this.notificationService.message('資料格式錯誤', SweetAlertIcon.error).then();
        break;
      }
    }
  }

  onDeviceSelectChange(event: any): void {
    const selected = event.detail.value as string;
    const selectedStr = selected || '';
    if (this.deviceSelected === selectedStr) { return; }
    this.deviceSelected = selectedStr;
    const device = this.availableDevices.find(x => x.deviceId === selected);
    this.deviceCurrent = device || undefined;
  }

  onDeviceChange(device: MediaDeviceInfo): void {
    const selectedStr = device?.deviceId || '';
    if (this.deviceSelected === selectedStr) { return; }
    this.deviceSelected = selectedStr;
    this.deviceCurrent = device || undefined;
  }

  onCamerasFound(devices: MediaDeviceInfo[]): void {
    this.availableDevices = devices;
    this.hasDevices = Boolean(devices && devices.length);
  }

  onHasPermission(has: boolean): void {
    this.hasPermission = has;
  }

}
