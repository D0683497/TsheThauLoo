import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { ActionSheetController, AlertController } from '@ionic/angular';
import { ModalService } from '../../../services/modal/modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IDocument } from '../../../models/document/document.model';
import { saveAs } from 'file-saver';
import { IGeneralCampaign } from '../../../models/activity/general-campaign/general-campaign.model';
import { GeneralCampaignService } from '../../../services/activity/general-campaign/general-campaign.service';
import { IGeneralCampaignEdit } from '../../../models/activity/general-campaign/general-campaign-edit.model';
import { ActivityType } from '../../../enums/activity-type.enum';
import { ICompany } from '../../../models/company/company.model';

@Component({
  selector: 'app-general-campaign-edit',
  templateUrl: './general-campaign-edit.component.html',
  styleUrls: ['./general-campaign-edit.component.scss'],
})
export class GeneralCampaignEditComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  generalId = this.route.snapshot.paramMap.get('generalId');
  generalCampaign: IGeneralCampaign;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  segment = 'info';
  urlRoot = environment.apiUrl;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private generalCampaignService: GeneralCampaignService,
    private route: ActivatedRoute,
    private actionSheetController: ActionSheetController,
    private modalService: ModalService,
    private alertController: AlertController) { }

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
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IGeneralCampaign): void {
    this.editForm = this.fb.group({
      title: [data.title, [Validators.required, Validators.maxLength(50)]],
      content: [data.content, [Validators.required]],
      declaration: [data.declaration],
      venue: [data.venue, [Validators.maxLength(200)]],
      registrationStartDate: [data.registrationStartTime],
      registrationStartTime: [data.registrationStartTime],
      registrationEndDate: [data.registrationEndTime],
      registrationEndTime: [data.registrationEndTime],
      startDate: [data.startTime, [Validators.required]],
      startTime: [data.startTime, [Validators.required]],
      endDate: [data.endTime, [Validators.required]],
      endTime: [data.endTime, [Validators.required]],
      limitNumberOfPeople: [data.limitNumberOfPeople, [Validators.required, Validators.min(0)]],
      enableVerify: [data.enableVerify, [Validators.required]],
      enableIdentityConfirmed: [data.enableIdentityConfirmed, [Validators.required]]
    });
  }

  async onSubmit(data: IGeneralCampaignEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.generalCampaignService.editGeneralCampaign(this.campaignId, this.generalId, data).subscribe(
      (res: IGeneralCampaign) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IGeneralCampaign): Promise<void> {
    await this.loadingService.end();
    this.generalCampaign = res;
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
        const errors: IServerError = err.error;
        await this.router.navigate(['/act']);
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
      }
    }
  }

  async option(data: IDocument): Promise<void> {
    const options = {
      translucent: true,
      header: '一般子活動附檔',
      buttons: [
        {
          text: '編輯',
          handler: () => {
            this.editFile(data);
          }
        },
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
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.deleteFile(data.id);
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

  async createFile(event: any): Promise<void> {
    const files = event.files;
    if (files.length > 0) {
      const file = files[0];
      if (file.size > 2147483647) {
        await this.notificationService.message('檔案過大', SweetAlertIcon.info);
        return;
      }
      await this.loadingService.start('上傳中...');
      this.generalCampaignService.createGeneralCampaignFile(this.campaignId, this.generalId, file).subscribe(
        (res: IDocument) => { this.createFileSuccess(res); },
        (err: HttpErrorResponse) => { this.createFileFail(err); }
      );
    }
  }

  async createFileSuccess(file: IDocument): Promise<void> {
    this.generalCampaign.files.push(file);
    await this.loadingService.end();
    await this.notificationService.toast('上傳成功', 2000, SweetAlertIcon.success);
  }

  async createFileFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
        await this.notificationService.toast('上傳失敗', 2000, SweetAlertIcon.error);
        break;
      case 403:
        const errors: IServerError = err.error;
        await this.router.navigate(['/act']);
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
    }
  }

  async editFile(data: IDocument): Promise<void> {
    const res = await this.modalService.editActivityFile(ActivityType.event, this.campaignId, this.generalId, data);
    if (res !== undefined) {
      const index = this.generalCampaign.files.findIndex(x => x.id === res.id);
      this.generalCampaign.files[index] = res;
    }
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

  async deleteFile(fileId: string): Promise<void> {
    await this.loadingService.start('刪除中...');
    this.generalCampaignService.deleteGeneralCampaignFile(this.campaignId, this.generalId, fileId).subscribe(
      () => { this.deleteFileSuccess(fileId); },
      (err: HttpErrorResponse) => { this.deleteFileFail(err); }
    );
  }

  async deleteFileSuccess(fileId: string): Promise<void> {
    const index = this.generalCampaign.files.findIndex(x => x.id === fileId);
    this.generalCampaign.files.splice(index, 1);
    await this.loadingService.end();
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteFileFail(err: HttpErrorResponse): Promise<void> {
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

  async deleteEvent(): Promise<void>{
    const alert = await this.alertController.create({
      header: '您是否要刪除此活動?',
      subHeader: '此操作無法復原',
      message: '若您確定要刪除此活動，系統將會發送通知信給所有已報名的使用者，通知此活動已被取消，並且所有活動附檔將會一起刪除',
      buttons: [
        {
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.loadingService.start('刪除中...');
            this.generalCampaignService.deleteGeneralCampaign(this.campaignId, this.generalId).subscribe(
              () => { this.deleteEventSuccess(); },
              (err: HttpErrorResponse) => { this.deleteEventFail(err); }
            );
          }
        },
        {
          text: '取消',
          role: 'cancel'
        },
      ]
    });
    await alert.present();
  }

  async deleteEventSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act']);
    await this.notificationService.toast('刪除成功', 2000, SweetAlertIcon.success);
  }

  async deleteEventFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        await this.notificationService.toast('查無此活動', 2000, SweetAlertIcon.error);
        break;
      case 400:
        await this.notificationService.toast('刪除失敗', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async signInEvent(): Promise<void> {
    await this.modalService.activitySignIn(this.campaignId, this.generalId, ActivityType.generalCampaign);
  }

  async participant(): Promise<void> {
    const uri = window.location.href.replace('edit', 'participate');
    await this.modalService.activityQRCode('活動現場報名', uri);
  }

  async inviteCompany(): Promise<void> {
    const companyId = await this.modalService.activityInviteCompany();
    if (companyId !== undefined && companyId !== null) {
      await this.loadingService.start('請稍候...');
      this.generalCampaignService.inviteGeneralCampaign(this.campaignId, this.generalId, companyId).subscribe(
        (res: ICompany) => { this.inviteSuccess(res); },
        (err: HttpErrorResponse) => { this.inviteFail(err); }
      );
    }
  }

  async inviteSuccess(res: ICompany): Promise<void> {
    await this.loadingService.end();
    await this.notificationService.toast('邀請成功', 2000, SweetAlertIcon.success);
  }

  async inviteFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 404:
        await this.notificationService.notify('邀請失敗', '無此公司', SweetAlertIcon.error);
        break;
    }
  }

  segmentChanged = (ev: any): void =>this.segment = ev.detail.value;

}
