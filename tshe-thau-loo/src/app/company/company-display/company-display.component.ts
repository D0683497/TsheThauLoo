import { Component, OnInit } from '@angular/core';
import { ICompany } from '../../models/company/company.model';
import { BehaviorSubject } from 'rxjs';
import { AccountService } from '../../services/account/account.service';
import { NotificationService } from '../../services/notification/notification.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';
import { HttpErrorResponse } from '@angular/common/http';
import { CompanyService } from '../../services/company/company.service';
import { createAvatar } from '@dicebear/avatars';
import * as style from '@dicebear/avatars-identicon-sprites';
import { environment } from '../../../environments/environment';
import { RoleType } from '../../enums/role-type.enum';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-company-display',
  templateUrl: './company-display.component.html',
  styleUrls: ['./company-display.component.scss'],
})
export class CompanyDisplayComponent implements OnInit {

  date = Date.now();
  companyId = this.route.snapshot.paramMap.get('companyId');
  company: ICompany;
  loading$ = new BehaviorSubject<boolean>(true);
  loadingError$ = new BehaviorSubject<boolean>(false);
  urlRoot = environment.apiUrl;
  type = RoleType;

  constructor(
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private companyService: CompanyService,
    private route: ActivatedRoute,
    public authService: AuthService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    await this.getData();
  }

  async getData(): Promise<void> {
    this.companyService.getCompany(this.companyId).subscribe(
      (res: ICompany) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: ICompany): Promise<void> {
    this.company = res;
    this.loading$.next(false);
    this.loadingError$.next(false);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    this.loadingError$.next(true);
    this.loading$.next(false);
  }

  createPhoto(id: string): string {
    return createAvatar(style, {
      seed: id,
      dataUri: true
    });
  }

  async logout(): Promise<void> {
    await this.accountService.logout();
    await this.notificationService.message('登出成功', SweetAlertIcon.success);
    await this.router.navigate(['/']);
  }

}
