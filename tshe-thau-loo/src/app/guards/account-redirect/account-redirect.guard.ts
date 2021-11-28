import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { RoleType } from '../../enums/role-type.enum';

@Injectable({
  providedIn: 'root'
})
export class AccountRedirectGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const role = this.authService.getUserRole();
    if (role === null) {
      this.router.navigate(['/account/login']).then();
    } else {
      switch (role) {
        case RoleType.administrator:
          this.router.navigate(['/account/profile/administrator']).then();
          break;
        case RoleType.alumnus:
          this.router.navigate(['/account/profile/alumnus']).then();
          break;
        case RoleType.employee:
          this.router.navigate(['/account/profile/employee']).then();
          break;
        case RoleType.examiner:
          this.router.navigate(['/account/profile/examiner']).then();
          break;
        case RoleType.manager:
          this.router.navigate(['/account/profile/manager']).then();
          break;
        case RoleType.student:
          this.router.navigate(['/account/profile/student']).then();
          break;
        default:
          this.router.navigate(['/account/login']).then();
          break;
      }
    }
    return true;
  }

}
