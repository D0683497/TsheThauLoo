import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { RoleType } from '../../enums/role-type.enum';
import { AuthService } from '../../services/auth/auth.service';
import { NotificationService } from '../../services/notification/notification.service';
import { SweetAlertIcon } from '../../enums/sweet-alert-icon.enum';

@Injectable({
  providedIn: 'root'
})
export class IsAdministratorGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router,
    private notificationService: NotificationService) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const role = this.authService.getUserRole();
    if (role === RoleType.administrator) {
      return true;
    } else {
      this.router.navigate(['/']).then();
      this.notificationService.message('沒有權限', SweetAlertIcon.warning).then();
      return false;
    }
  }

}
