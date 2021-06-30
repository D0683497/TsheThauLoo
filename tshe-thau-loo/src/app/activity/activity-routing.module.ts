import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ActivityHomeComponent } from './activity-home/activity-home.component';
import { EventListComponent } from './event/event-list/event-list.component';
import { EventCreateComponent } from './event/event-create/event-create.component';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';
import { IsAdministratorGuard } from '../guards/is-administrator/is-administrator.guard';

const routes: Routes = [
  { path: '', component: ActivityHomeComponent, pathMatch: 'full' },
  {
    path: 'event',
    children: [
      { path: 'list', component: EventListComponent, pathMatch: 'full' },
      { path: 'create', component: EventCreateComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsAdministratorGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ActivityRoutingModule { }
