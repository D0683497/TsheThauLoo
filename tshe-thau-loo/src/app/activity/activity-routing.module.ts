import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityHomeComponent } from './activity-home/activity-home.component';
import { EventListComponent } from './event/event-list/event-list.component';
import { EventCreateComponent } from './event/event-create/event-create.component';
import { EventEditComponent } from './event/event-edit/event-edit.component';
import { EventDisplayComponent } from './event/event-display/event-display.component';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';
import { IsAdministratorGuard } from '../guards/is-administrator/is-administrator.guard';
import { MyEventListComponent } from './my/my-event-list/my-event-list.component';
import { MyEventComponent } from './my/my-event/my-event.component';
import { EventParticipateComponent } from './event/event-participate/event-participate.component';
import { CampaignDisplayComponent } from './campaign/campaign-display/campaign-display.component';
import { CampaignEditComponent } from './campaign/campaign-edit/campaign-edit.component';
import { CampaignCreateComponent } from './campaign/campaign-create/campaign-create.component';
import { CampaignListComponent } from './campaign/campaign-list/campaign-list.component';
import { GeneralCampaignListComponent } from './general-campaign/general-campaign-list/general-campaign-list.component';
import { GeneralCampaignCreateComponent } from './general-campaign/general-campaign-create/general-campaign-create.component';
import { GeneralCampaignEditComponent } from './general-campaign/general-campaign-edit/general-campaign-edit.component';
import { GeneralCampaignDisplayComponent } from './general-campaign/general-campaign-display/general-campaign-display.component';
// eslint-disable-next-line max-len
import { GeneralCampaignParticipateComponent } from './general-campaign/general-campaign-participate/general-campaign-participate.component';
import { MyCampaignListComponent } from './my/my-campaign-list/my-campaign-list.component';
import { MyCampaignComponent } from './my/my-campaign/my-campaign.component';
import { MyGeneralCampaignComponent } from './my/my-general-campaign/my-general-campaign.component';
import { RecruitmentCampaignListComponent } from './recruitment-campaign/recruitment-campaign-list/recruitment-campaign-list.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignCreateComponent } from './recruitment-campaign/recruitment-campaign-create/recruitment-campaign-create.component';
import { RecruitmentCampaignEditComponent } from './recruitment-campaign/recruitment-campaign-edit/recruitment-campaign-edit.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignDisplayComponent } from './recruitment-campaign/recruitment-campaign-display/recruitment-campaign-display.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningCreateComponent } from './recruitment-campaign/recruitment-campaign-opening-create/recruitment-campaign-opening-create.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningEditComponent } from './recruitment-campaign/recruitment-campaign-opening-edit/recruitment-campaign-opening-edit.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningListComponent } from './recruitment-campaign/recruitment-campaign-opening-list/recruitment-campaign-opening-list.component';
// eslint-disable-next-line max-len
import { RecruitmentCampaignOpeningDisplayComponent } from './recruitment-campaign/recruitment-campaign-opening-display/recruitment-campaign-opening-display.component';

const routes: Routes = [
  { path: '', component: ActivityHomeComponent, pathMatch: 'full' },
  {
    path: 'event',
    children: [
      { path: 'list', component: EventListComponent, pathMatch: 'full' },
      {
        path: 'create',
        component: EventCreateComponent,
        pathMatch: 'full',
        canActivate: [RequiredLoginGuard, IsAdministratorGuard]
      },
      {
        path: ':eventId/edit',
        component: EventEditComponent,
        pathMatch: 'full',
        canActivate: [RequiredLoginGuard, IsAdministratorGuard]
      },
      { path: ':eventId/participate', component: EventParticipateComponent, pathMatch: 'full' },
      { path: ':eventId', component: EventDisplayComponent, pathMatch: 'full' }
    ]
  },
  {
    path: 'campaign',
    children: [
      { path: 'list', component: CampaignListComponent, pathMatch: 'full' },
      {
        path: 'create',
        component: CampaignCreateComponent,
        pathMatch: 'full',
        canActivate: [RequiredLoginGuard, IsAdministratorGuard]
      },
      {
        path: ':campaignId/edit',
        component: CampaignEditComponent,
        pathMatch: 'full',
        canActivate: [RequiredLoginGuard, IsAdministratorGuard]
      },
      { path: ':campaignId', component: CampaignDisplayComponent, pathMatch: 'full' },
      {
        path: ':campaignId/general',
        children: [
          { path: 'list', component: GeneralCampaignListComponent, pathMatch: 'full' },
          {
            path: 'create',
            component: GeneralCampaignCreateComponent,
            pathMatch: 'full',
            canActivate: [RequiredLoginGuard, IsAdministratorGuard]
          },
          {
            path: ':generalId/edit',
            component: GeneralCampaignEditComponent,
            pathMatch: 'full',
            canActivate: [RequiredLoginGuard, IsAdministratorGuard]
          },
          { path: ':generalId', component: GeneralCampaignDisplayComponent, pathMatch: 'full' },
          { path: ':generalId/participate', component: GeneralCampaignParticipateComponent, pathMatch: 'full' },
        ]
      },
      {
        path: ':campaignId/recruitment',
        children: [
          { path: 'list', component: RecruitmentCampaignListComponent, pathMatch: 'full' },
          {
            path: 'create',
            component: RecruitmentCampaignCreateComponent,
            pathMatch: 'full',
            canActivate: [RequiredLoginGuard, IsAdministratorGuard]
          },
          {
            path: ':recruitmentId/edit',
            component: RecruitmentCampaignEditComponent,
            pathMatch: 'full',
            canActivate: [RequiredLoginGuard, IsAdministratorGuard]
          },
          { path: ':recruitmentId', component: RecruitmentCampaignDisplayComponent, pathMatch: 'full' },
          {
            path: ':recruitmentId/opening',
            children: [
              { path: 'list', component: RecruitmentCampaignOpeningListComponent, pathMatch: 'full' },
              { path: 'create', component: RecruitmentCampaignOpeningCreateComponent, pathMatch: 'full' },
              { path: ':openingId/edit', component: RecruitmentCampaignOpeningEditComponent, pathMatch: 'full' },
              { path: ':openingId', component: RecruitmentCampaignOpeningDisplayComponent, pathMatch: 'full' },
            ]
          }
        ]
      }
    ]
  },
  {
    path: 'my',
    children: [
      { path: 'event', component: MyEventListComponent, pathMatch: 'full' },
      { path: 'event/:eventId', component: MyEventComponent, pathMatch: 'full' },
      { path: 'campaign', component: MyCampaignListComponent, pathMatch: 'full' },
      { path: 'campaign/:campaignId', component: MyCampaignComponent, pathMatch: 'full' },
      { path: 'campaign/:campaignId/general/:generalId', component: MyGeneralCampaignComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ActivityRoutingModule { }
