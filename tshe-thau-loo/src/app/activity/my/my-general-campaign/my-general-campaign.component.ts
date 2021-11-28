import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AttendeeStatusType } from '../../../enums/attendee-status-type.enum';
import { MyService } from '../../../services/my/my.service';
import { AuthService } from '../../../services/auth/auth.service';
import { ModalService } from '../../../services/modal/modal.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivityType } from '../../../enums/activity-type.enum';
import { ActivityActionType } from '../../../enums/activity-action-type.enum';
import { IMyGeneralCampaign } from '../../../models/activity/my-campaign/my-general-campaign.model';

@Component({
  selector: 'app-my-general-campaign',
  templateUrl: './my-general-campaign.component.html',
  styleUrls: ['./my-general-campaign.component.scss'],
})
export class MyGeneralCampaignComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  generalId = this.route.snapshot.paramMap.get('generalId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  generalCampaign: IMyGeneralCampaign;
  canShowQRCode = false;
  type = AttendeeStatusType;

  constructor(
    private route: ActivatedRoute,
    private myService: MyService,
    private authService: AuthService,
    private modalService: ModalService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.myService.myGeneral(this.campaignId, this.generalId).subscribe(
      (res: IMyGeneralCampaign) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IMyGeneralCampaign): Promise<void> {
    this.generalCampaign = res;
    if (new Date(res.startTime).getTime() <= this.date &&
      new Date(res.endTime).getTime() > this.date &&
      res.status === AttendeeStatusType.signUpSuccess) {
      this.canShowQRCode = true;
    }
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async showQRCode(): Promise<void> {
    const uri = JSON.stringify({
      userId: this.authService.getUserId(),
      firstId: this.campaignId,
      secondId: this.generalId,
      type: ActivityType.generalCampaign,
      action: ActivityActionType.attendee
    });
    const data = await this.modalService.activityQRCode('活動票卷', uri);
    if (data) {
      await this.getData();
    }
  }

}
