import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RoleType } from '../../../enums/role-type.enum';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IRecruitmentCampaign } from '../../../models/activity/recruitment-campaign/recruitment-campaign.model';
import { RecruitmentCampaignService } from '../../../services/activity/recruitment-campaign/recruitment-campaign.service';

@Component({
  selector: 'app-recruitment-campaign-list',
  templateUrl: './recruitment-campaign-list.component.html',
  styleUrls: ['./recruitment-campaign-list.component.scss'],
})
export class RecruitmentCampaignListComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  recruitmentCampaigns: IRecruitmentCampaign[];
  type = RoleType;

  constructor(
    private route: ActivatedRoute,
    private recruitmentCampaignService: RecruitmentCampaignService,
    public authService: AuthService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.recruitmentCampaignService.getRecruitmentCampaigns(this.campaignId).subscribe(
      (res: IRecruitmentCampaign[]) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IRecruitmentCampaign[]): Promise<void> {
    this.recruitmentCampaigns = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
