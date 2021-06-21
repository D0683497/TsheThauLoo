import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyHomeComponent } from './company-home/company-home.component';
import { CompanyCreateComponent } from './company-create/company-create.component';
import { IsManagerGuard } from '../guards/is-manager/is-manager.guard';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';

const routes: Routes = [
  { path: '', component: CompanyHomeComponent, pathMatch: 'full' },
  { path: 'create', component: CompanyCreateComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsManagerGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyRoutingModule { }
