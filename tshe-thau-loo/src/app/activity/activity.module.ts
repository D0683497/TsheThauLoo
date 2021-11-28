import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivityRoutingModule } from './activity-routing.module';
import { ActivityHomeComponent } from './activity-home/activity-home.component';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { QuillModule } from 'ngx-quill';
import * as QuillBlotFormatter from 'quill-blot-formatter';
import { EventListComponent } from './event/event-list/event-list.component';
import { EventCreateComponent } from './event/event-create/event-create.component';
import { EventEditComponent } from './event/event-edit/event-edit.component';
import { EventDisplayComponent } from './event/event-display/event-display.component';
import { ActivityFileEditComponent } from './activity-file-edit/activity-file-edit.component';
import { ActivityDeclarationComponent } from './activity-declaration/activity-declaration.component';
import { MyEventListComponent } from './my/my-event-list/my-event-list.component';
import { MyEventComponent } from './my/my-event/my-event.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { ActivitySignInComponent } from './activity-sign-in/activity-sign-in.component';
import { ActivityQrCodeComponent } from './activity-qr-code/activity-qr-code.component';
import { EventParticipateComponent } from './event/event-participate/event-participate.component';
import { CampaignCreateComponent } from './campaign/campaign-create/campaign-create.component';
import { CampaignDisplayComponent } from './campaign/campaign-display/campaign-display.component';
import { CampaignEditComponent } from './campaign/campaign-edit/campaign-edit.component';
import { CampaignListComponent } from './campaign/campaign-list/campaign-list.component';
import { GeneralCampaignCreateComponent } from './general-campaign/general-campaign-create/general-campaign-create.component';
import { GeneralCampaignDisplayComponent } from './general-campaign/general-campaign-display/general-campaign-display.component';
import { GeneralCampaignEditComponent } from './general-campaign/general-campaign-edit/general-campaign-edit.component';
import { GeneralCampaignListComponent } from './general-campaign/general-campaign-list/general-campaign-list.component';
// eslint-disable-next-line max-len
import { GeneralCampaignParticipateComponent } from './general-campaign/general-campaign-participate/general-campaign-participate.component';
import { ActivityInviteCompanyComponent } from './activity-invite-company/activity-invite-company.component';
import { MyCampaignComponent } from './my/my-campaign/my-campaign.component';
import { MyGeneralCampaignComponent } from './my/my-general-campaign/my-general-campaign.component';
import { MyCampaignListComponent } from './my/my-campaign-list/my-campaign-list.component';
import { RecruitmentCampaignListComponent } from './recruitment-campaign/recruitment-campaign-list/recruitment-campaign-list.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignCreateComponent } from './recruitment-campaign/recruitment-campaign-create/recruitment-campaign-create.component';
import { RecruitmentCampaignEditComponent } from './recruitment-campaign/recruitment-campaign-edit/recruitment-campaign-edit.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignDisplayComponent } from './recruitment-campaign/recruitment-campaign-display/recruitment-campaign-display.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningListComponent } from './recruitment-campaign/recruitment-campaign-opening-list/recruitment-campaign-opening-list.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningCreateComponent } from './recruitment-campaign/recruitment-campaign-opening-create/recruitment-campaign-opening-create.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningEditComponent } from './recruitment-campaign/recruitment-campaign-opening-edit/recruitment-campaign-opening-edit.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningDisplayComponent } from './recruitment-campaign/recruitment-campaign-opening-display/recruitment-campaign-opening-display.component';
import { FacultyCreateComponent } from './faculty/faculty-create/faculty-create.component';
import { FacultyEditComponent } from './faculty/faculty-edit/faculty-edit.component';
import { QualificationCreateComponent } from './qualification/qualification-create/qualification-create.component';
import { QualificationEditComponent } from './qualification/qualification-edit/qualification-edit.component';
import { ResumeSelectComponent } from './resume-select/resume-select.component';
import { DeliveryResumeListComponent } from './delivery-resume-list/delivery-resume-list.component';

@NgModule({
  declarations: [
    ActivityHomeComponent,
    EventCreateComponent,
    EventListComponent,
    EventDisplayComponent,
    EventEditComponent,
    ActivityFileEditComponent,
    ActivityDeclarationComponent,
    MyEventListComponent,
    MyEventComponent,
    ActivitySignInComponent,
    ActivityQrCodeComponent,
    EventParticipateComponent,
    CampaignCreateComponent,
    CampaignDisplayComponent,
    CampaignEditComponent,
    CampaignListComponent,
    GeneralCampaignCreateComponent,
    GeneralCampaignDisplayComponent,
    GeneralCampaignEditComponent,
    GeneralCampaignListComponent,
    GeneralCampaignParticipateComponent,
    ActivityInviteCompanyComponent,
    MyCampaignComponent,
    MyGeneralCampaignComponent,
    MyCampaignListComponent,
    RecruitmentCampaignListComponent,
    RecruitmentCampaignCreateComponent,
    RecruitmentCampaignEditComponent,
    RecruitmentCampaignDisplayComponent,
    RecruitmentCampaignOpeningListComponent,
    RecruitmentCampaignOpeningCreateComponent,
    RecruitmentCampaignOpeningEditComponent,
    RecruitmentCampaignOpeningDisplayComponent,
    FacultyCreateComponent,
    FacultyEditComponent,
    QualificationCreateComponent,
    QualificationEditComponent,
    ResumeSelectComponent,
    DeliveryResumeListComponent
  ],
  imports: [
    CommonModule,
    ActivityRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    QuillModule.forRoot({
      modules: {
        syntax: false, // 程式碼語法檢測
        toolbar: [
          [{ header: [1, 2, 3, 4, 5, 6, false] }], // 標題大小
          ['bold', 'italic', 'underline', 'strike'],
          [{ list: 'ordered'}, { list: 'bullet' }],
          [{ align: [] }],
          [{ indent: '-1'}, { indent: '+1' }],
          [{ color: [] }, { background: [] }],
          ['blockquote', 'code-block'],
          ['link', 'image'],
          ['clean'],
        ],
        blotFormatter: {}
      },
      customModules: [{
        implementation: QuillBlotFormatter.default,
        path: 'modules/blotFormatter'
      }],
    }),
    ZXingScannerModule
  ]
})
export class ActivityModule { }
