import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company/company.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { ICompany } from '../../models/company/company.model';
import { Pagination } from '../../models/pagination/pagination';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-identicon-sprites';

@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.scss'],
})
export class CompanyListComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  companies: ICompany[];
  pagination = new Pagination();
  urlRoot = environment.apiUrl;

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private companyService: CompanyService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.companyService.getCompanies(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<ICompany[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<ICompany[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.companies = res.body as ICompany[];
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async next(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex + this.pagination.pageSize;
    await this.getData();
  }

  async previous(): Promise<void> {
    this.pagination.pageIndex = this.pagination.pageIndex - this.pagination.pageSize;
    await this.getData();
  }

  createPhoto(id: string): string {
    return createAvatar(style, {
      seed: id,
      dataUri: true
    });
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
