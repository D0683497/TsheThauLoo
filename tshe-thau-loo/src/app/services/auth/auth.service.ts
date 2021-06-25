import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserInfo } from '../../models/user-info.model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Storage } from '@ionic/storage-angular';
import { RoleType } from '../../enums/role-type.enum';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  userInfo$ = new BehaviorSubject<UserInfo | null>(null);
  helper = new JwtHelperService();
  token: string = null;

  constructor(private storage: Storage) { }

  init(): void {
    this.storage.create().then();
    this.storage.get('access_token').then((res: string) => {
      if (res !== null) {
        this.setLoginStatus(res);
      }
      if (this.isTokenExpired()) {
        this.removeLoginStatus();
      }
    });
  }

  // 設置登入狀態
  setLoginStatus(token: string): void {
    this.storage.set('access_token', token).then();
    this.token = token;
    const decodeToken = this.helper.decodeToken(token);
    this.userInfo$.next(new UserInfo(decodeToken));
  }

  // 移除登入狀態 (登出)
  removeLoginStatus(): void {
    this.storage.remove('access_token').then();
    this.userInfo$.next(null);
  }

  // 獲取登入狀態
  isLogin = (): boolean => this.userInfo$.getValue() !== null;

  getToken = (): string => this.token;

  // 獲取使用者角色
  getUserRole(): RoleType | null {
    const user = this.userInfo$.getValue();
    return user !== null ? user.role : null;
  }

  // 獲取使用者識別碼
  getUserId(): string | null {
    const user = this.userInfo$.getValue();
    return user !== null ? user.nameId : null;
  }

  isTokenExpired = (): boolean => this.helper.isTokenExpired(this.token);

}
