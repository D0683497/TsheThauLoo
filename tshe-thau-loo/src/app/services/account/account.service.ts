import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { ILogin } from '../../models/account/login/login.model';
import { Observable } from 'rxjs';
import { ILoginResponse } from '../../models/account/login/login-response.model';
import { map } from 'rxjs/operators';
import { IChangeUserName } from '../../models/account/change-user-name.models';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private authService: AuthService) { }

  // 登入
  login(data: ILogin): Observable<void> {
    const url = `${this.urlRoot}/account/login`;
    return this.http.post<ILoginResponse>(url, data, this.httpOptions).pipe(
      map((result) => {
        this.authService.setLoginStatus(result.accessToken);
      })
    );
  }

  // 登出
  async logout(): Promise<void> {
    await this.authService.removeLoginStatus();
  }

  changeUserName(data: IChangeUserName): Observable<void> {
    const url = `${this.urlRoot}/account/username`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

}
