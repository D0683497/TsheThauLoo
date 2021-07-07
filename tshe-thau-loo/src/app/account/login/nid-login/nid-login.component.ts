import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NotificationService } from '../../../services/notification/notification.service';
import { SweetAlertIcon } from '../../../enums/sweet-alert-icon.enum';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-nid-login',
  templateUrl: './nid-login.component.html',
  styleUrls: ['./nid-login.component.scss'],
})
export class NidLoginComponent implements OnInit {

  date = Date.now();

  constructor(
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private router: Router,
    private authService: AuthService) { }

  ngOnInit(): void {}

  async ionViewWillEnter(): Promise<void> {
    if (this.route.snapshot.queryParamMap.has('token')) {
      const token = decodeURIComponent(this.route.snapshot.queryParamMap.get('token') as string);
      this.authService.setLoginStatus(token);
      await this.notificationService.toast('登入成功', 2000, SweetAlertIcon.success);
      await this.router.navigate(['/']);
    } else {
      await this.notificationService.message('登入失敗', SweetAlertIcon.error);
      await this.router.navigate(['/account/login']);
    }
  }

}
