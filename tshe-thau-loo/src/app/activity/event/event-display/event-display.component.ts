import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { IEvent } from '../../../models/activity/event/event.model';
import { HttpErrorResponse } from '@angular/common/http';
import { EventService } from '../../../services/activity/event/event.service';
import { RoleType } from '../../../enums/role-type.enum';
import { AuthService } from '../../../services/auth/auth.service';

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
    public authService: AuthService) { }

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
    if (res.registrationEndTime !== null) {
      this.canAttendee = new Date(res.registrationEndTime).getTime() > this.date;
    } else {
      this.canAttendee = new Date(res.startTime).getTime() > this.date;
    }
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

}
