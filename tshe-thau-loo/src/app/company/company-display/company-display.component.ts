import { Component, OnInit } from '@angular/core';
import { ICompany } from '../../models/company/company.model';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { CompanyService } from '../../services/company/company.service';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-identicon-sprites';

@Component({
  selector: 'app-company-display',
  templateUrl: './company-display.component.html',
  styleUrls: ['./company-display.component.scss'],
})
export class CompanyDisplayComponent implements OnInit {

  date = Date.now();
  companyId = this.route.snapshot.paramMap.get('companyId');
  company: ICompany;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  photo: string;

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private companyService: CompanyService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.companyService.getCompany(this.companyId).subscribe(
      (res: ICompany) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: ICompany): Promise<void> {
    this.company = res;
    if (res.hasLogo) {
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

  async downloadPhoto(): Promise<void> {
    this.companyService.getLogo(this.companyId).subscribe(
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

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
