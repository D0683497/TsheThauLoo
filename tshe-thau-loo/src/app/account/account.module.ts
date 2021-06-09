import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { IonicModule } from '@ionic/angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SelectRegisterComponent } from './register/select-register/select-register.component';
import { AdministratorRegisterComponent } from './register/administrator-register/administrator-register.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TermsRegisterComponent } from './register/terms-register/terms-register.component';
import { AlumnusRegisterComponent } from './register/alumnus-register/alumnus-register.component';

@NgModule({
  declarations: [
    SelectRegisterComponent,
    AdministratorRegisterComponent,
    TermsRegisterComponent,
    AlumnusRegisterComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule,
    IonicModule,
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ]
})
export class AccountModule { }
