import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../../services/account/account.service';
import { NotificationService } from '../../../services/notification/notification.service';
import { IConfirmEmail } from '../../../models/account/email/confirm-email.model';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { LoadingService } from '../../../services/loading/loading.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {

  date = Date.now();

  constructor(
    private route: ActivatedRoute,
    private accountService: AccountService,
    private notificationService: NotificationService,
    private router: Router,
    private loadingService: LoadingService) { }

  async ionViewWillEnter(): Promise<void> {
    if (this.route.snapshot.queryParamMap.has('userId') && this.route.snapshot.queryParamMap.has('token')) {
      const data: IConfirmEmail = {
        userId: decodeURIComponent(this.route.snapshot.queryParamMap.get('userId') as string),
        token: decodeURIComponent(this.route.snapshot.queryParamMap.get('token') as string)
      };
      await this.confirm(data);
    } else {
      await this.router.navigate(['/']);
      await this.notificationService.message('網址錯誤', SweetAlertIcon.error);
    }
  }

  async confirm(data: IConfirmEmail): Promise<void> {
    await this.loadingService.start('驗證中...');
    this.accountService.confirmEmail(data).subscribe(
      () => { this.confirmSuccess(); },
      () => { this.confirmFail(); }
    );
  }

  async confirmSuccess(): Promise<void> {
    await this.loadingService.end();
    await this.router.navigate(['/account/login']);
    await this.notificationService.message('驗證成功', SweetAlertIcon.success);
  }

  async confirmFail(): Promise<void> {
    await this.loadingService.end();
    await this.notificationService.message('驗證失敗', SweetAlertIcon.error);
  }

  ngOnInit(): void {}

}
