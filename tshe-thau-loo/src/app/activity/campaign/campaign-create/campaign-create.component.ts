import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../../../services/notification/notification.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { CampaignService } from '../../../services/activity/campaign/campaign.service';
import { ICampaignCreate } from '../../../models/activity/campaign/campaign-create.model';
import { ICampaign } from '../../../models/activity/campaign/campaign.model';

@Component({
  selector: 'app-campaign-create',
  templateUrl: './campaign-create.component.html',
  styleUrls: ['./campaign-create.component.scss'],
})
export class CampaignCreateComponent implements OnInit {

  year = new Date().getFullYear();
  createForm: FormGroup;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private campaignService: CampaignService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      title: [null, [Validators.required, Validators.maxLength(50)]],
      content: [null, [Validators.required]],
      startDate: [null, [Validators.required]],
      startTime: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      endTime: [null, [Validators.required]]
    });
  }

  async onSubmit(data: ICampaignCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.campaignService.createCampaign(data).subscribe(
      (res: ICampaign) => { this.registerSuccess(res); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  async registerSuccess(res: ICampaign): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act/campaign', res.id, 'edit']);
    await this.notificationService.notify('建立成功', '即將前往系列活動編輯頁面', SweetAlertIcon.success);
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
