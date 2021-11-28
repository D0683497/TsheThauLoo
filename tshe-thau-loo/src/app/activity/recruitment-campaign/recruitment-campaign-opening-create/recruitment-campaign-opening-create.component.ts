import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OpeningService } from '../../../services/opening/opening.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { IOpeningCreate } from '../../../models/opening/opening-create.model';
import { IOpening } from '../../../models/opening/opening.model';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { IFormError } from '../../../models/error/form-error.model';
import { IServerError } from '../../../models/error/server-error.model';
import { NotificationService } from '../../../services/notification/notification.service';
import { LoadingService } from '../../../services/loading/loading.service';
import { EducationType } from '../../../enums/education-type.enum';

@Component({
  selector: 'app-recruitment-campaign-opening-create',
  templateUrl: './recruitment-campaign-opening-create.component.html',
  styleUrls: ['./recruitment-campaign-opening-create.component.scss'],
})
export class RecruitmentCampaignOpeningCreateComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  recruitmentId = this.route.snapshot.paramMap.get('recruitmentId');
  createForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private openingService: OpeningService,
    private fb: FormBuilder,
    private router: Router,
    private notificationService: NotificationService,
    private loadingService: LoadingService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      divisionName: [null, [Validators.required, Validators.maxLength(30)]],
      jobTitle: [null, [Validators.required, Validators.maxLength(30)]],
      jobDescription: [null, [Validators.required]],
      workPlace: [null, [Validators.required, Validators.maxLength(200)]],
      salary: [null, [Validators.required, Validators.maxLength(20)]],
      requiredNumber: [0, [Validators.required, Validators.min(0)]],
      education: [EducationType.none, [Validators.required]],
      workExperience: [null, [Validators.required, Validators.maxLength(500)]],
      language: [null, [Validators.required, Validators.maxLength(100)]],
      nationality: [null, [Validators.required, Validators.maxLength(20)]],
      isAccessibility: [false, [Validators.required]]
    });
  }

  async onSubmit(data: IOpeningCreate): Promise<void> {
    await this.loadingService.start('建立中...');
    this.openingService.createOpening(this.campaignId, this.recruitmentId, data).subscribe(
      (res: IOpening) => { this.createSuccess(res); },
      (err: HttpErrorResponse) => { this.createFail(err); }
    );
  }

  async createSuccess(res: IOpening): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/act/campaign', this.campaignId, 'recruitment', this.recruitmentId, 'opening', res.id, 'edit']);
    await this.notificationService.notify('建立成功', '即將前往職缺編輯頁面', SweetAlertIcon.success);
  }

  async createFail(err: HttpErrorResponse): Promise<void> {
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
