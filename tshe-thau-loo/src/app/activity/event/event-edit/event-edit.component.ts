import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { EventService } from '../../../services/activity/event/event.service';
import { HttpErrorResponse } from '@angular/common/http';
import { IEvent } from '../../../models/activity/event/event.model';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { IEventEdit } from '../../../models/activity/event/event-edit.model';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.scss'],
})
export class EventEditComponent implements OnInit {

  date = Date.now();
  eventId = this.route.snapshot.paramMap.get('eventId');
  event: IEvent;
  editForm: FormGroup;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  segment = 'info';

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private eventService: EventService,
    private route: ActivatedRoute) { }

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
    this.buildForm(res);
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  buildForm(data: IEvent): void {
    this.editForm = this.fb.group({
      title: [data.title, [Validators.required, Validators.maxLength(50)]],
      content: [data.content, [Validators.required]],
      declaration: [data.declaration],
      venue: [data.venue, [Validators.maxLength(200)]],
      registrationStartDate: [data.registrationStartTime],
      registrationStartTime: [data.registrationStartTime],
      registrationEndDate: [data.registrationEndTime],
      registrationEndTime: [data.registrationEndTime],
      startDate: [data.startTime, [Validators.required]],
      startTime: [data.startTime, [Validators.required]],
      endDate: [data.endTime, [Validators.required]],
      endTime: [data.endTime, [Validators.required]],
      limitNumberOfPeople: [data.limitNumberOfPeople, [Validators.required, Validators.min(0)]],
      enableVerify: [data.enableVerify, [Validators.required]],
      enableIdentityConfirmed: [data.enableIdentityConfirmed, [Validators.required]]
    });
  }

  async onSubmit(data: IEventEdit): Promise<void> {
    await this.loadingService.start('修改中...');
    this.eventService.editEvent(this.eventId, data).subscribe(
      (res: IEvent) => { this.editSuccess(res); },
      (err: HttpErrorResponse) => { this.editFail(err); }
    );
  }

  async editSuccess(res: IEvent): Promise<void> {
    await this.loadingService.end();
    this.event = res;
    await this.notificationService.toast('修改成功', 2000, SweetAlertIcon.success);
  }

  async editFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.editForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('修改失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const errors: IServerError = err.error;
        await this.router.navigate(['/act/event', this.eventId]);
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
      }
    }
  }

  segmentChanged = (ev: CustomEvent): void =>this.segment = ev.detail.value;

}
