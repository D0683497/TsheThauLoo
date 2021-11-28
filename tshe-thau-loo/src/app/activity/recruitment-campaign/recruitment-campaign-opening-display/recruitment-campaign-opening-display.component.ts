import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { RoleType } from '../../../enums/role-type.enum';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IOpening } from '../../../models/opening/opening.model';
import { OpeningService } from '../../../services/opening/opening.service';
import { ModalService } from '../../../services/modal/modal.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';

@Component({
  selector: 'app-recruitment-campaign-opening-display',
  templateUrl: './recruitment-campaign-opening-display.component.html',
  styleUrls: ['./recruitment-campaign-opening-display.component.scss'],
})
export class RecruitmentCampaignOpeningDisplayComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  recruitmentId = this.route.snapshot.paramMap.get('recruitmentId');
  openingId = this.route.snapshot.paramMap.get('openingId');
  opening: IOpening;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  type = RoleType;

  constructor(
    private route: ActivatedRoute,
    private openingService: OpeningService,
    public authService: AuthService,
    private modalService: ModalService,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.openingService.getOpening(this.campaignId, this.recruitmentId, this.openingId).subscribe(
      (res: IOpening) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IOpening): Promise<void> {
    this.opening = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async delivery(): Promise<void> {
    const res = await this.modalService.deliveryResume(this.campaignId, this.recruitmentId, this.openingId);
    if (res !== undefined) {
      await this.loadingService.start('投遞中...');
      this.openingService.deliveryResume(this.campaignId, this.recruitmentId, this.openingId, res).subscribe(
        () => { this.deliverySuccess(); },
        (err: HttpErrorResponse) => { this.deliveryFail(err); }
      );
    }
  }

  async deliverySuccess(): Promise<void> {
    await this.loadingService.end();
    await this.notificationService.message('投遞成功', SweetAlertIcon.success);
  }

  async deliveryFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    await this.notificationService.message('投遞失敗', SweetAlertIcon.error);
  }

}
