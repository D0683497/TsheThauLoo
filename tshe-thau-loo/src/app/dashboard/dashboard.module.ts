import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { HomeComponent } from './home/home.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatListModule } from '@angular/material/list';
import { PrivacyStatementComponent } from './privacy-statement/privacy-statement.component';
import { PrivacyProtectionComponent } from './privacy-protection/privacy-protection.component';
import { PrivacyRightsComponent } from './privacy-rights/privacy-rights.component';
import { PrivacyAnnouncementComponent } from './privacy-announcement/privacy-announcement.component';

@NgModule({
  declarations: [
    HomeComponent,
    PrivacyStatementComponent,
    PrivacyProtectionComponent,
    PrivacyRightsComponent,
    PrivacyAnnouncementComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    MatButtonModule,
    MatDividerModule,
    FontAwesomeModule,
    MatListModule
  ]
})
export class DashboardModule { }
