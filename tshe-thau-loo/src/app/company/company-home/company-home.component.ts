import { Component, OnInit } from '@angular/core';
import { MyService } from '../../services/my/my.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ICompany } from '../../models/company/company.model';
import { Router } from '@angular/router';
import { NotificationService } from '../../services/notification/notification.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';

@Component({
  selector: 'app-company-home',
  templateUrl: './company-home.component.html',
  styleUrls: ['./company-home.component.scss'],
})
export class CompanyHomeComponent implements OnInit {

  date = Date.now();

  constructor(
    private myService: MyService,
    private router: Router,
    private notificationService: NotificationService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    this.myService.company().subscribe(
      (res: ICompany) => { this.getSuccess(res); },
      (err: HttpErrorResponse) => { this.getFail(err); }
    );
  }

  async getSuccess(res: ICompany): Promise<void> {
    await this.router.navigate(['/company', res.id]);
  }

  async getFail(err: HttpErrorResponse): Promise<void> {
    switch (err.status) {
      case 404:
        await this.notificationService.message('即將前往建立公司頁面', SweetAlertIcon.info);
        await this.router.navigate(['/company/create']);
        break;
      case 403:
        await this.router.navigate(['/company/list']);
        break;
    }
  }

}
