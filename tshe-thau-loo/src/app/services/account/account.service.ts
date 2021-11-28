import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/auth.service';
import { ILogin } from '../../models/account/login/login.model';
import { Observable } from 'rxjs';
import { ILoginResponse } from '../../models/account/login/login-response.model';
import { map } from 'rxjs/operators';
import { IChangeUserName } from '../../models/account/change-user-name.models';
import { IChangeEmail } from '../../models/account/email/change-email.model';
import { IConfirmEmail } from '../../models/account/email/confirm-email.model';
import { IChangePhone } from '../../models/account/change-phone.models';
import { IChangePassword } from '../../models/account/password/change-password.models';
import { IForgetPassword } from '../../models/account/password/forget-password.models';
import { IResetPassword } from '../../models/account/password/reset-password.models';
import { INational } from '../../models/account/national/national.model';
import { INationalEdit } from '../../models/account/national/national-edit.model';
import { INationalVerify } from '../../models/account/national/national-verify.model';
import { INationalEditVerify } from '../../models/account/national/national-edit-verify.model';
import { IDocument } from '../../models/document/document.model';
import { IDocumentEdit } from '../../models/document/document-edit.model';

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

  nidLogin = (): string => `${this.urlRoot}/account/nid-login`;

  // 登出
  async logout(): Promise<void> {
    await this.authService.removeLoginStatus();
  }

  changeUserName(data: IChangeUserName): Observable<void> {
    const url = `${this.urlRoot}/account/username`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  changeEmail(data: IChangeEmail): Observable<void> {
    const url = `${this.urlRoot}/account/email`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  confirmEmail(data: IConfirmEmail): Observable<void> {
    const url = `${this.urlRoot}/account/email/confirm`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  changePhone(data: IChangePhone): Observable<void> {
    const url = `${this.urlRoot}/account/phone`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  changePassword(data: IChangePassword): Observable<void> {
    const url = `${this.urlRoot}/account/password`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  forgetPassword(data: IForgetPassword): Observable<void> {
    const url = `${this.urlRoot}/account/password/forget`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  resetPassword(data: IResetPassword): Observable<void> {
    const url = `${this.urlRoot}/account/password/reset`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  getNational(): Observable<INational> {
    const url = `${this.urlRoot}/account/national`;
    return this.http.get<INational>(url, this.httpOptions);
  }

  editNational(data: INationalEdit): Observable<INational> {
    const url = `${this.urlRoot}/account/national`;
    return this.http.post<INational>(url, data, this.httpOptions);
  }

  getNationalVerify(): Observable<INationalVerify> {
    const url = `${this.urlRoot}/account/national/verify`;
    return this.http.get<INationalVerify>(url, this.httpOptions);
  }

  editNationalVerify(data: INationalEditVerify): Observable<INationalVerify> {
    const url = `${this.urlRoot}/account/national/verify`;
    return this.http.post<INationalVerify>(url, data, this.httpOptions);
  }

  createNationalVerifyFile(file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/account/national/verify/files`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  editNationalVerifyFile(fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/account/national/verify/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  getNationalVerifyFile(fileId: string): Observable<IDocument> {
    const url = `${this.urlRoot}/account/national/verify/files/${fileId}`;
    return this.http.get<IDocument>(url, this.httpOptions);
  }

  downloadNationalVerifyFile(fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/account/national/verify/files/${fileId}/download`;
    return this.http.get(url, { responseType: 'blob' });
  }

  deleteNationalVerifyFile(fileId: string): Observable<void> {
    const url = `${this.urlRoot}/account/national/verify/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  getPhoto(): Observable<Blob> {
    const url = `${this.urlRoot}/account/photo`;
    return this.http.get(url, { responseType: 'blob' });
  }

  createPhoto(file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/account/photo`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  deletePhoto(): Observable<void> {
    const url = `${this.urlRoot}/account/photo`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
