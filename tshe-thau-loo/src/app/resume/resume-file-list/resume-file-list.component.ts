import { Component, OnInit } from '@angular/core';
import { IDocument } from '../../models/document/document.model';
import { ResumeService } from '../../services/resume/resume.service';
import { BehaviorSubject } from 'rxjs';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { NotificationService } from '../../services/notification/notification.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { saveAs } from 'file-saver';
import { LoadingService } from '../../services/loading/loading.service';
import { Pagination } from '../../models/pagination/pagination';
import { ModalService } from '../../services/modal/modal.service';
import { ActivatedRoute } from '@angular/router';
import { ActionSheetController } from '@ionic/angular';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-resume-file-list',
  templateUrl: './resume-file-list.component.html',
  styleUrls: ['./resume-file-list.component.scss'],
})
export class ResumeFileListComponent implements OnInit {

  date = Date.now();
  archive = false;
  resumes: IDocument[];
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  urlRoot = environment.apiUrl;
  userId = this.authService.getUserId();

  constructor(
    private route: ActivatedRoute,
    private resumeService: ResumeService,
    private notificationService: NotificationService,
    private loadingService: LoadingService,
    private modalService: ModalService,
    private actionSheetController: ActionSheetController,
    private authService: AuthService) { }

  ngOnInit(): void { }

  async ionViewWillEnter(): Promise<void> {
    if (this.route.snapshot.queryParamMap.has('archive')) {
      this.archive = this.route.snapshot.queryParamMap.get('archive').toLocaleLowerCase() === 'true';
    }
    await this.getData();
  }

  async getData(): Promise<void> {
    this.resumeService.getResumes(this.pagination.pageIndex, this.pagination.pageSize, this.archive).subscribe(
      (res: HttpResponse<IDocument[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<IDocument[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.resumes = res.body as IDocument[];
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async create(event: any): Promise<void> {
    const files = event.files;
    if (files.length > 0) {
      const file = files[0];
      if (file.size > 2147483647) {
        await this.notificationService.message('檔案過大', SweetAlertIcon.info);
        return;
      }
      await this.loadingService.start('上傳中...');
      this.resumeService.createResume(file).subscribe(
        (res: IDocument) => { this.createSuccess(res); },
        (err: HttpErrorResponse) => { this.createFail(err); }
      );
    }
  }

  async createSuccess(file: IDocument): Promise<void> {
    this.resumes.push(file);
    await this.loadingService.end();
    await this.notificationService.toast('上傳成功', 2000, SweetAlertIcon.success);
  }

  async createFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
        await this.notificationService.toast('上傳失敗', 2000, SweetAlertIcon.error);
        break;
    }
  }

  async option(data: IDocument): Promise<void> {
    const options = {
      translucent: true,
      header: '檔案履歷',
      buttons: [
        {
          text: '編輯',
          handler: () => {
            this.edit(data);
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
            const url = `https://docs.google.com/viewer?url=${this.urlRoot}/resumes/${data.id}?userId=${this.userId}`;
            window.open(url, '_blank');
          }
        },
        {
          text: '刪除',
          role: 'destructive',
          handler: () => {
            this.delete(data.id);
          }
        },
        {
          text: '取消',
          icon: 'close',
          role: 'cancel',
        }
      ]
    };
    if (this.archive) {
      options.buttons.splice(options.buttons.findIndex(x => x.text === '編輯'), 1);
      options.buttons.splice(options.buttons.findIndex(x => x.text === '刪除'), 1);
    }
    const actionSheet = await this.actionSheetController.create(options);
    await actionSheet.present();
  }

  async download(fileId: string, fileName: string): Promise<void> {
    await this.loadingService.start('下載中...');
    this.resumeService.getResume(fileId).subscribe(
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

  async edit(data: IDocument): Promise<void> {
    const res = await this.modalService.editFileResume(data);
    if (res !== undefined) {
      const index = this.resumes.findIndex(x => x.id === res.id);
      this.resumes[index] = res;
    }
  }

  async delete(fileId: string): Promise<void> {
    await this.loadingService.start('刪除中...');
    this.resumeService.deleteResume(fileId).subscribe(
      () => { this.deleteSuccess(fileId); },
      (err: HttpErrorResponse) => { this.deleteFail(err); }
    );
  }

  async deleteSuccess(fileId: string): Promise<void> {
    const index = this.resumes.findIndex(x => x.id === fileId);
    this.resumes.splice(index, 1);
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

  async next(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex + this.pagination.pageSize;
    await this.getData();
  }

  async previous(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex - this.pagination.pageSize;
    await this.getData();
  }

}
