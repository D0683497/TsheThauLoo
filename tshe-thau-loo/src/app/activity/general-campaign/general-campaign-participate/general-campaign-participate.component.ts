import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../../../services/modal/modal.service';
import { PhoneNumberValidator } from '../../../validators/phone.validator';
import { ActivityType } from '../../../enums/activity-type.enum';
import { ActivityActionType } from '../../../enums/activity-action-type.enum';

@Component({
  selector: 'app-general-campaign-participate',
  templateUrl: './general-campaign-participate.component.html',
  styleUrls: ['./general-campaign-participate.component.scss'],
})
export class GeneralCampaignParticipateComponent implements OnInit {

  date = Date.now();
  campaignId = this.route.snapshot.paramMap.get('campaignId');
  generalId = this.route.snapshot.paramMap.get('generalId');
  returnUrl = this.router.createUrlTree(['/act/campaign', this.campaignId, 'general', this.generalId]).toString();
  createForm: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private modalService: ModalService) { }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      name: [null, [Validators.required, Validators.maxLength(50)]],
      contactPhone: [null, [Validators.required, Validators.maxLength(30), PhoneNumberValidator('TW')]],
      remark: [null, [Validators.maxLength(500)]]
    });
  }

  async onSubmit(data: {name: string; contactPhone: string; remark: string}): Promise<void> {
    const uri = JSON.stringify({
      firstId: this.campaignId,
      secondId: this.generalId,
      type: ActivityType.generalCampaign,
      action: ActivityActionType.participant,
      name: data.name,
      contactPhone: data.contactPhone,
      remark: data.remark
    });
    await this.modalService.activityQRCode('活動票卷', uri);
  }

}
