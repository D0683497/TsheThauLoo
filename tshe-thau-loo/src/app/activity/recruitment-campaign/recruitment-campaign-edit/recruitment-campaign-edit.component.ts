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
import { ActivityType } from '../../../enums/activity-type.enum';
import { saveAs } from 'file-saver';
import { ICompany } from '../../../models/company/company.model';
import { RecruitmentCampaignService } from '../../../services/activity/recruitment-campaign/recruitment-campaign.service';
import { IRecruitmentCampaign } from '../../../models/activity/recruitment-campaign/recruitment-campaign.model';
import { IRecruitmentCampaignEdit } from '../../../models/activity/recruitment-campaign/recruitment-campaign-edit.model';

@Component({
  selector: 'app-recruitment-campaign-edit',
  templateUrl: './recruitment-campaign-edit.component.html',
  styleUrls: ['./recruitment-campaign-edit.component.scss'],
})
export class RecruitmentCampaignEditComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  recruitmentId = this.route.snapshot.paramMap.get('recruitmentId');
  recruitmentCampaign: IRecruitmentCampaign;
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
    private recruitmentCampaignService: RecruitmentCampaignService,
    private route: ActivatedRoute,
    private actionSheetController: ActionSheetController,
    private modalService: ModalService,
    private alertController: AlertController) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.recruitmentCampaignService.getRecruitmentCampaign(this.campaignId, this.recruitmentId).subscribe(
      (res: IRecruitmentCampaign) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IRecruitmentCampaign): Promise<void> {
    this.recruitmentCampaign = res;
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IRecruitmentCampaign): void {
    this.editForm = this.fb.group({
      title: [data.title, [Validators.required, Validators.maxLength(50)]],
      content: [data.content, [Validators.required]],
      startDate: [data.startTime, [Validators.required]],
      startTime: [data.startTime, [Validators.required]],
      endDate: [data.endTime, [Validators.required]],
      endTime: [data.endTime, [Validators.required]],
      enableReview: [data.enableReview, [Validators.required]]
    });
  }

  async onSubmit(data: IRecruitmentCampaignEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.recruitmentCampaignService.editRecruitmentCampaign(this.campaignId, this.recruitmentId, data).subscribe(
      (res: IRecruitmentCampaign) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IRecruitmentCampaign): Promise<void> {
    await this.loadingService.end();
    this.recruitmentCampaign = res;
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
      header: '徵才子活動附檔',
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
                          ${this.urlRoot}/campaigns/${this.campaignId}/recruitment/${this.recruitmentId}/files/${data.id}`;
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
      this.recruitmentCampaignService.createRecruitmentCampaignFile(this.campaignId, this.recruitmentId, file).subscribe(
        (res: IDocument) => { this.createFileSuccess(res); },
        (err: HttpErrorResponse) => { this.createFileFail(err); }
      );
    }
  }

  async createFileSuccess(file: IDocument): Promise<void> {
    this.recruitmentCampaign.files.push(file);
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
    const res = await this.modalService.editActivityFile(ActivityType.recruitmentCampaign, this.campaignId, this.recruitmentId, data);
    if (res !== undefined) {
      const index = this.recruitmentCampaign.files.findIndex(x => x.id === res.id);
      this.recruitmentCampaign.files[index] = res;
    }
  }

  async download(fileId: string, fileName: string): Promise<void> {
    await this.loadingService.start('下載中...');
    this.recruitmentCampaignService.getRecruitmentCampaignFile(this.campaignId, this.recruitmentId, fileId).subscribe(
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
    this.recruitmentCampaignService.deleteRecruitmentCampaignFile(this.campaignId, this.recruitmentId, fileId).subscribe(
      () => { this.deleteFileSuccess(fileId); },
      (err: HttpErrorResponse) => { this.deleteFileFail(err); }
    );
  }

  async deleteFileSuccess(fileId: string): Promise<void> {
    const index = this.recruitmentCampaign.files.findIndex(x => x.id === fileId);
    this.recruitmentCampaign.files.splice(index, 1);
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
      message: '若您確定要刪除此活動，所有活動附檔將會一起刪除',
      buttons: [
        {
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.loadingService.start('刪除中...');
            this.recruitmentCampaignService.deleteRecruitmentCampaign(this.campaignId, this.recruitmentId).subscribe(
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

  async inviteCompany(): Promise<void> {
    const companyId = await this.modalService.activityInviteCompany();
    if (companyId !== undefined && companyId !== null) {
      await this.loadingService.start('請稍候...');
      this.recruitmentCampaignService.inviteRecruitmentCampaign(this.campaignId, this.recruitmentId, companyId).subscribe(
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
