import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { MyService } from '../../../services/my/my.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IMyEvent } from '../../../models/activity/my-event/my-event.model';
import { AuthService } from '../../../services/auth/auth.service';
import { ModalService } from '../../../services/modal/modal.service';
import { ActivityType } from '../../../enums/activity-type.enum';
import { AttendeeStatusType } from '../../../enums/attendee-status-type.enum';
import { ActivityActionType } from '../../../enums/activity-action-type.enum';

@Component({
  selector: 'app-my-event',
  templateUrl: './my-event.component.html',
  styleUrls: ['./my-event.component.scss'],
})
export class MyEventComponent implements OnInit {

  date = Date.now();
  eventId = this.route.snapshot.paramMap.get('eventId');
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  event: IMyEvent;
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
    this.myService.myEvent(this.eventId).subscribe(
      (res: IMyEvent) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IMyEvent): Promise<void> {
    this.event = res;
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
      firstId: this.eventId,
      secondId: null,
      type: ActivityType.event,
      action: ActivityActionType.attendee
    });
    const data = await this.modalService.activityQRCode('活動票卷', uri);
    if (data) {
      await this.getData();
    }
  }

}
