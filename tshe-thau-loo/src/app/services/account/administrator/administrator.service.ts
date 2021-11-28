import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAdministratorRegister } from '../../../models/account/register/administrator-register.model';
import { Observable } from 'rxjs';
import { IAdministratorProfile } from '../../../models/account/profile/administrator/administrator-profile.model';
import { IAdministratorInfo } from '../../../models/account/profile/administrator/administrator-info.model';
import { IAdministratorEditInfo } from '../../../models/account/profile/administrator/administrator-edit-info.model';
import { IResponsibilityCreate } from '../../../models/account/profile/administrator/responsibility-create.model';
import { IResponsibilityEdit } from '../../../models/account/profile/administrator/responsibility-edit.model';

@Injectable({
  providedIn: 'root'
})
export class AdministratorService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IAdministratorRegister): Observable<IAdministratorProfile> {
    const url = `${this.urlRoot}/account/administrator/register`;
    return this.http.post<IAdministratorProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IAdministratorProfile> {
    const url = `${this.urlRoot}/account/administrator/profile`;
    return this.http.get<IAdministratorProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IAdministratorInfo> {
    const url = `${this.urlRoot}/account/administrator/profile/info`;
    return this.http.get<IAdministratorInfo>(url, this.httpOptions);
  }

  editInfo(data: IAdministratorEditInfo): Observable<IAdministratorInfo> {
    const url = `${this.urlRoot}/account/administrator/profile/info`;
    return this.http.post<IAdministratorInfo>(url, data, this.httpOptions);
  }

  createResponsibility(data: IResponsibilityCreate): Observable<IAdministratorInfo> {
    const url = `${this.urlRoot}/account/administrator/profile/info/responsibilities`;
    return this.http.post<IAdministratorInfo>(url, data, this.httpOptions);
  }

  editResponsibility(responsibilityId: string, data: IResponsibilityEdit): Observable<IAdministratorInfo> {
    const url = `${this.urlRoot}/account/administrator/profile/info/responsibilities/${responsibilityId}`;
    return this.http.post<IAdministratorInfo>(url, data, this.httpOptions);
  }

  deleteResponsibility(responsibilityId: string): Observable<IAdministratorInfo> {
    const url = `${this.urlRoot}/account/administrator/profile/info/responsibilities/${responsibilityId}`;
    return this.http.delete<IAdministratorInfo>(url, this.httpOptions);
  }

}
