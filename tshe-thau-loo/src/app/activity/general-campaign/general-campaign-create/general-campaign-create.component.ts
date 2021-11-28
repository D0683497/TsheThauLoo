import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { GeneralCampaignService } from '../../../services/activity/general-campaign/general-campaign.service';
import { IGeneralCampaignCreate } from '../../../models/activity/general-campaign/general-campaign-create.model';
import { IEvent } from '../../../models/activity/event/event.model';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';

@Component({
  selector: 'app-general-campaign-create',
  templateUrl: './general-campaign-create.component.html',
  styleUrls: ['./general-campaign-create.component.scss'],
})
export class GeneralCampaignCreateComponent implements OnInit {

  year = new Date().getFullYear();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  createForm: FormGroup;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private generalCampaignService: GeneralCampaignService,
    private route: ActivatedRoute) { }

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

  async onSubmit(data: IGeneralCampaignCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.generalCampaignService.createGeneralCampaign(this.campaignId, data).subscribe(
      (res: IEvent) => { this.registerSuccess(res); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  async registerSuccess(res: IEvent): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act/campaign', this.campaignId, 'general', res.id, 'edit']);
    await this.notificationService.notify('建立成功', '即將前往一般子活動編輯頁面', SweetAlertIcon.success);
  }

  async registerFail(err: HttpErrorResponse): Promise<void> {
    await this.loadingService.end();
    switch (err.status) {
      case 400:
      {
        const errors: IFormError[] = err.error;
        errors.forEach(element => {
          this.createForm.get(element.propertyName)?.setErrors({server: element.errorMessage});
        });
        await this.notificationService.message('建立失敗', SweetAlertIcon.error);
        break;
      }
      case 403:
      {
        const errors: IServerError = err.error;
        await this.router.navigate(['/act']);
        await this.notificationService.notify(errors.title, errors.detail, SweetAlertIcon.error);
        break;
      }
    }
  }

}
