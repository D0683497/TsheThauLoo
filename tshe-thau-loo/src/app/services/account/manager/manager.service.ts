import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IManagerRegister } from '../../../models/account/register/manager-register.model';
import { Observable } from 'rxjs';
import { IManagerProfile } from '../../../models/account/profile/manager/manager-profile.model';
import { IManagerInfo } from '../../../models/account/profile/manager/manager-info.model';
import { IManagerEditInfo } from '../../../models/account/profile/manager/manager-edit-info.model';
import { ISubstituteEdit } from '../../../models/account/profile/manager/substitute-edit.model';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IManagerRegister): Observable<IManagerProfile> {
    const url = `${this.urlRoot}/account/manager/register`;
    return this.http.post<IManagerProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IManagerProfile> {
    const url = `${this.urlRoot}/account/manager/profile`;
    return this.http.get<IManagerProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IManagerInfo> {
    const url = `${this.urlRoot}/account/manager/profile/info`;
    return this.http.get<IManagerInfo>(url, this.httpOptions);
  }

  editInfo(data: IManagerEditInfo): Observable<IManagerInfo> {
    const url = `${this.urlRoot}/account/manager/profile/info`;
    return this.http.post<IManagerInfo>(url, data, this.httpOptions);
  }

  editSubstitute(data: ISubstituteEdit): Observable<IManagerInfo> {
    const url = `${this.urlRoot}/account/manager/profile/info/substitute`;
    return this.http.post<IManagerInfo>(url, data, this.httpOptions);
  }

}
