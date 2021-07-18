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

const routes: Routes = [
  { path: '', component: ActivityHomeComponent, pathMatch: 'full' },
  {
    path: 'event',
    children: [
      { path: 'list', component: EventListComponent, pathMatch: 'full' },
      { path: 'create', component: EventCreateComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsAdministratorGuard] },
      { path: ':eventId/edit', component: EventEditComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsAdministratorGuard] },
      { path: ':eventId/participate', component: EventParticipateComponent, pathMatch: 'full' },
      { path: ':eventId', component: EventDisplayComponent, pathMatch: 'full' }
    ]
  },
  {
    path: 'campaign',
    children: [
      { path: 'list', component: CampaignListComponent, pathMatch: 'full' },
      { path: 'create', component: CampaignCreateComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsAdministratorGuard] },
      {
        path: ':campaignId/edit',
        component: CampaignEditComponent,
        pathMatch: 'full',
        canActivate: [RequiredLoginGuard, IsAdministratorGuard]
      },
      { path: ':campaignId', component: CampaignDisplayComponent, pathMatch: 'full' }
    ]
  },
  {
    path: 'my',
    children: [
      { path: 'event', component: MyEventListComponent, pathMatch: 'full' },
      { path: 'event/:eventId', component: MyEventComponent, pathMatch: 'full' },
    ],
    canActivate: [RequiredLoginGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ActivityRoutingModule { }
