import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { NotificationService } from '../../services/notification/notification.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';

@Injectable({
  providedIn: 'root'
})
export class RequiredLoginGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authService.isLogin()) {
      return true;
    } else {
      this.router.navigate(['/account/login']).then();
      this.notificationService.message('請先登入', SweetAlertIcon.warning).then();
      return false;
    }
  }

}
