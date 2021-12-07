import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppHeaderComponent } from './app-header/app-header.component';
import { AppFooterComponent } from './app-footer/app-footer.component';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
  declarations: [
    AppHeaderComponent,
    AppFooterComponent,
    NotFoundComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatToolbarModule,
    FontAwesomeModule
  ],
  exports: [
    AppHeaderComponent,
    AppFooterComponent,
    NotFoundComponent
  ]
})
export class SharedModule { }
