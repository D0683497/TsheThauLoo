import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotificationService } from '../../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '../../../services/loading/loading.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { RecruitmentCampaignService } from '../../../services/activity/recruitment-campaign/recruitment-campaign.service';
import { IRecruitmentCampaignCreate } from '../../../models/activity/recruitment-campaign/recruitment-campaign-create.model';
import { IRecruitmentCampaign } from '../../../models/activity/recruitment-campaign/recruitment-campaign.model';

@Component({
  selector: 'app-recruitment-campaign-create',
  templateUrl: './recruitment-campaign-create.component.html',
  styleUrls: ['./recruitment-campaign-create.component.scss'],
})
export class RecruitmentCampaignCreateComponent implements OnInit {

  year = new Date().getFullYear();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  createForm: FormGroup;

  constructor(
    private notificationService: NotificationService,
    private router: Router,
    private fb: FormBuilder,
    private loadingService: LoadingService,
    private recruitmentCampaignService: RecruitmentCampaignService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      title: [null, [Validators.required, Validators.maxLength(50)]],
      content: [null, [Validators.required]],
      startDate: [null, [Validators.required]],
      startTime: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      endTime: [null, [Validators.required]],
      enableReview: [false, [Validators.required]]
    });
  }

  async onSubmit(data: IRecruitmentCampaignCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.recruitmentCampaignService.createRecruitmentCampaign(this.campaignId, data).subscribe(
      (res: IRecruitmentCampaign) => { this.registerSuccess(res); },
      (err: HttpErrorResponse) => { this.registerFail(err); }
    );
  }

  async registerSuccess(res: IRecruitmentCampaign): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act/campaign', this.campaignId, 'recruitment', res.id, 'edit']);
    await this.notificationService.notify('建立成功', '即將前往徵才子活動編輯頁面', SweetAlertIcon.success);
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
