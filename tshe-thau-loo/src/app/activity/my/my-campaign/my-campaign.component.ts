import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AttendeeStatusType } from '../../../enums/attendee-status-type.enum';
import { ActivatedRoute } from '@angular/router';
import { MyService } from '../../../services/my/my.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IMyCampaign } from '../../../models/activity/my-campaign/my-campaign.model';

@Component({
  selector: 'app-my-campaign',
  templateUrl: './my-campaign.component.html',
  styleUrls: ['./my-campaign.component.scss'],
})
export class MyCampaignComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  campaign: IMyCampaign;
  type = AttendeeStatusType;

  constructor(
    private route: ActivatedRoute,
    private myService: MyService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.myService.myCampaign(this.campaignId).subscribe(
      (res: IMyCampaign) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IMyCampaign): Promise<void> {
    this.campaign = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
