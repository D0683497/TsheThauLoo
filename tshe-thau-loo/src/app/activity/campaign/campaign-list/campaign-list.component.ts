import { Component, OnInit } from '@angular/core';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { BehaviorSubject } from 'rxjs';
import { Pagination } from '../../../models/pagination/pagination';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { ICampaign } from '../../../models/activity/campaign/campaign.model';
import { CampaignService } from '../../../services/activity/campaign/campaign.service';

@Component({
  selector: 'app-campaign-list',
  templateUrl: './campaign-list.component.html',
  styleUrls: ['./campaign-list.component.scss'],
})
export class CampaignListComponent implements OnInit {

  date = Date.now();
  status = ActivityStatus.ing;
  type = ActivityStatus;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  pagination = new Pagination();
  campaigns: ICampaign[];

  constructor(
    private route: ActivatedRoute,
    private campaignService: CampaignService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    if (this.route.snapshot.queryParamMap.has('status')) {
      switch (this.route.snapshot.queryParamMap.get('status').toLocaleLowerCase()) {
        case 'coming':
          this.status = ActivityStatus.coming;
          break;
        case 'ing':
          this.status = ActivityStatus.ing;
          break;
        case 'end':
          this.status = ActivityStatus.end;
          break;
        default:
          this.status = ActivityStatus.ing;
          break;
      }
    } else {
      this.status = ActivityStatus.ing;
    }
    await this.getData();
  }

  async getData(): Promise<void> {
    this.campaignService.getCampaigns(this.pagination.pageIndex, this.pagination.pageSize, this.status).subscribe(
      (res: HttpResponse<ICampaign[]>) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: HttpResponse<ICampaign[]>): Promise<void> {
    const pagination = JSON.parse(res.headers.get('X-Pagination') as string);
    this.pagination.pageLength = pagination.pageLength;
    this.pagination.pageSize = pagination.pageSize;
    this.pagination.pageIndex = pagination.pageIndex;
    this.campaigns = res.body as ICampaign[];
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
