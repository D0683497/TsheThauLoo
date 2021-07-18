import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { IGeneralCampaign } from '../../../models/activity/general-campaign/general-campaign.model';
import { GeneralCampaignService } from '../../../services/activity/general-campaign/general-campaign.service';
import { AuthService } from '../../../services/auth/auth.service';
import { RoleType } from '../../../enums/role-type.enum';

@Component({
  selector: 'app-general-campaign-list',
  templateUrl: './general-campaign-list.component.html',
  styleUrls: ['./general-campaign-list.component.scss'],
})
export class GeneralCampaignListComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  generalCampaigns: IGeneralCampaign[];
  type = RoleType;

  constructor(
    private route: ActivatedRoute,
    private generalCampaignService: GeneralCampaignService,
    public authService: AuthService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.generalCampaignService.getGeneralCampaigns(this.campaignId).subscribe(
      (res: IGeneralCampaign[]) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IGeneralCampaign[]): Promise<void> {
    this.generalCampaigns = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
