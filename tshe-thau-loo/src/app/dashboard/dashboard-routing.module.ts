import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { DashboardAboutComponent } from './dashboard-about/dashboard-about.component';
import { DashboardPrivacyComponent } from './dashboard-privacy/dashboard-privacy.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const routes: Routes = [
  { path: 'home', component: DashboardHomeComponent, pathMatch: 'full' },
  { path: 'about', component: DashboardAboutComponent, pathMatch: 'full' },
  { path: 'privacy', component: DashboardPrivacyComponent, pathMatch: 'full' },
  { path: 'page-not-found', component: PageNotFoundComponent, pathMatch: 'full' },
  { path: '', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
