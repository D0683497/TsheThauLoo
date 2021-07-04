import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { IEvent } from '../../../models/activity/event/event.model';
import { HttpErrorResponse } from '@angular/common/http';
import { EventService } from '../../../services/activity/event/event.service';
import { RoleType } from '../../../enums/role-type.enum';
import { AuthService } from '../../../services/auth/auth.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { NotificationService } from '../../../services/notification/notification.service';
import { IServerError } from '../../../models/error/server-error.model';

@Component({
  selector: 'app-event-display',
  templateUrl: './event-display.component.html',
  styleUrls: ['./event-display.component.scss'],
})
export class EventDisplayComponent implements OnInit {

  date = Date.now();
  eventId = this.route.snapshot.paramMap.get('eventId');
  event: IEvent;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  type = RoleType;
  canAttendee = false;

  constructor(
    private route: ActivatedRoute,
    private eventService: EventService,
    public authService: AuthService,
    private loadingService: LoadingService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.eventService.getEvent(this.eventId).subscribe(
      (res: IEvent) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: IEvent): Promise<void> {
    this.event = res;
    if (res.registrationStartTime !== null && res.registrationEndTime !== null) {
      if (new Date(res.registrationStartTime).getTime() < this.date && new Date(res.registrationEndTime).getTime() > this.date) {
        this.canAttendee = true;
      }
    } else {
      if (new Date(res.startTime).getTime() > this.date) {
        this.canAttendee = true;
      }
    }
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  async attendee(): Promise<void> {
    await this.loadingService.start('報名中...');
    this.eventService.attendeeEvent(this.eventId).subscribe(
      () => { this.attendeeSuccess(); },
      (err: HttpErrorResponse) => { this.attendeeFail(err); }
    );
  }

  async attendeeSuccess(): Promise<void> {
    await this.loadingService.end();
    if (this.event.enableVerify) {
      await this.notificationService.notify('已送出報名', '請等待管理員審核', SweetAlertIcon.success);
    } else {
      await this.notificationService.message('報名成功', SweetAlertIcon.success);
    }
  }

  async attendeeFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 403) {
      const errors: IServerError = err.error;
      this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error).then();
    }
  }

}
