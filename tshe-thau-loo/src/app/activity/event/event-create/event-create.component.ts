import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { EventService } from '../../../services/activity/event/event.service';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { IFormError } from '../../../models/error/form-error.model';
import { IEventCreate } from '../../../models/activity/event/event-create.model';
import { IEvent } from '../../../models/activity/event/event.model';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.scss'],
})
export class EventCreateComponent implements OnInit {

  year = new Date().getFullYear();
  createForm: FormGroup;

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private eventService: EventService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      title: [null, [Validators.required, Validators.maxLength(50)]],
      content: [null, [Validators.required]],
      declaration: [null],
      venue: [null, [Validators.maxLength(200)]],
      registrationStartDate: [null],
      registrationStartTime: [null],
      registrationEndDate: [null],
      registrationEndTime: [null],
      startDate: [null, [Validators.required]],
      startTime: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      endTime: [null, [Validators.required]],
      limitNumberOfPeople: [0, [Validators.required, Validators.min(0)]],
      enableVerify: [false, [Validators.required]],
      enableIdentityConfirmed: [false, [Validators.required]]
    });
  }

  async onSubmit(data: IEventCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.eventService.createEvent(data).subscribe(
      (res: IEvent) => { this.registerSuccess(res); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  async registerSuccess(res: IEvent): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act/event', res.id]);
    await this.notificationService.notify('建立成功', '即將前往一般活動編輯頁面', SweetAlertIcon.success);
  }

  async registerFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    if (err.status === 400) {
      const errors: IFormError[] = err.error;
      errors.forEach(element => {
        this.createForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
      });
      await this.notificationService.message('註冊失敗', SweetAlertIcon.error);
    }
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
