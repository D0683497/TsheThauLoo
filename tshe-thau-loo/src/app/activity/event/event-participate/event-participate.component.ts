import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PhoneNumberValidator } from '../../../validators/phone.validator';
import { ActivityType } from '../../../enums/activity-type.enum';
import { ActivityActionType } from '../../../enums/activity-action-type.enum';
import { ModalService } from '../../../services/modal/modal.service';

@Component({
  selector: 'app-event-participate',
  templateUrl: './event-participate.component.html',
  styleUrls: ['./event-participate.component.scss'],
})
export class EventParticipateComponent implements OnInit {

  date = Date.now();
  eventId = this.route.snapshot.paramMap.get('eventId');
  returnUrl = this.router.createUrlTree(['/act/event', this.eventId]).toString();
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
      firstId: this.eventId,
      secondId: null,
      type: ActivityType.event,
      action: ActivityActionType.participant,
      name: data.name,
      contactPhone: data.contactPhone,
      remark: data.remark
    });
    await this.modalService.activityQRCode('活動票卷', uri);
  }

}
