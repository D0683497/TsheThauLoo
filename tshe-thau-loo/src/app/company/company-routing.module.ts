import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyHomeComponent } from './company-home/company-home.component';
import { CompanyCreateComponent } from './company-create/company-create.component';
import { IsManagerGuard } from '../guards/is-manager/is-manager.guard';
import { RequiredLoginGuard } from '../guards/required-login/required-login.guard';
import { CompanyDisplayComponent } from './company-display/company-display.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { CompanyListComponent } from './company-list/company-list.component';

const routes: Routes = [
  { path: '', component: CompanyHomeComponent, pathMatch: 'full' },
  { path: 'list', component: CompanyListComponent, pathMatch: 'full' },
  { path: 'create', component: CompanyCreateComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsManagerGuard] },
  { path: ':companyId', component: CompanyDisplayComponent, pathMatch: 'full' },
  { path: ':companyId/edit', component: CompanyEditComponent, pathMatch: 'full', canActivate: [RequiredLoginGuard, IsManagerGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompanyRoutingModule { }
