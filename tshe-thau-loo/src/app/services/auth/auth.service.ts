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

  async init(): Promise<void> {
    await this.storage.create();
    const token = await this.storage.get('access_token');
    if (token !== null) {
      await this.setLoginStatus(token);
    }
    if (this.isTokenExpired()) {
      await this.removeLoginStatus();
    }
  }

  // 設置登入狀態
  async setLoginStatus(token: string): Promise<void> {
    await this.storage.set('access_token', token);
    this.token = token;
    const decodeToken = this.helper.decodeToken(token);
    this.userInfo$.next(new UserInfo(decodeToken));
  }

  // 移除登入狀態 (登出)
  async removeLoginStatus(): Promise<void> {
    await this.storage.remove('access_token');
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

  isTokenExpired = (): boolean => this.helper.isTokenExpired(this.token);

}
