import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdministratorRegisterComponent } from './register/administrator-register/administrator-register.component';
import { SelectRegisterComponent } from './register/select-register/select-register.component';

const routes: Routes = [
  {
    path: 'register',
    children: [
      { path: '', component: SelectRegisterComponent, pathMatch: 'full' },
      { path: 'administrator', component: AdministratorRegisterComponent, pathMatch: 'full' }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
