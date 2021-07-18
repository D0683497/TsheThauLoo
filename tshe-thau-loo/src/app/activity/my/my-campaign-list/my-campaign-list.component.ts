import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../../../models/pagination/pagination';
import { ActivatedRoute } from '@angular/router';
import { MyService } from '../../../services/my/my.service';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { IMyCampaign } from '../../../models/activity/my-campaign/my-campaign.model';

@Component({
  selector: 'app-my-campaign-list',
  templateUrl: './my-campaign-list.component.html',
  styleUrls: ['./my-campaign-list.component.scss'],
})
export class MyCampaignListComponent implements OnInit {

  date = Date.now();
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  campaigns: IMyCampaign[];

  constructor(
    private route: ActivatedRoute,
    private myService: MyService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.myService.myCampaigns(this.pagination.pageIndex, this.pagination.pageSize).subscribe(
      (res: HttpResponse<IMyCampaign[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<IMyCampaign[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.campaigns = res.body as IMyCampaign[];
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

}
