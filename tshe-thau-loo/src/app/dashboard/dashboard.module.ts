import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { DashboardAboutComponent } from './dashboard-about/dashboard-about.component';
import { DashboardPrivacyComponent } from './dashboard-privacy/dashboard-privacy.component';
import { IonicModule } from '@ionic/angular';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

@NgModule({
  declarations: [
    DashboardHomeComponent,
    DashboardAboutComponent,
    DashboardPrivacyComponent,
    PageNotFoundComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    IonicModule
  ]
})
export class DashboardModule { }
