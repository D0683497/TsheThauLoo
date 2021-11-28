import { Component, OnInit } from '@angular/core';
import { ModalController } from '@ionic/angular';
import { BehaviorSubject } from 'rxjs';
import { ICompany } from '../../models/company/company.model';
import { Pagination } from '../../models/pagination/pagination';
import { CompanyService } from '../../services/company/company.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-identicon-sprites';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-activity-invite-company',
  templateUrl: './activity-invite-company.component.html',
  styleUrls: ['./activity-invite-company.component.scss'],
})
export class ActivityInviteCompanyComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  companies: ICompany[];
  pagination = new Pagination();
  urlRoot = environment.apiUrl;

  constructor(
    private modalController: ModalController,
    private companyService: CompanyService) { }

  async ngOnInit(): Promise<void> {
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

  async dismiss(companyId: string): Promise<void> {
    await this.modalController.dismiss(companyId);
  }

}
