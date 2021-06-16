import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IEmployeeRegister } from '../../../models/account/register/employee-register.model';
import { Observable } from 'rxjs';
import { IEmployeeProfile } from '../../../models/account/profile/employee/employee-profile.model';
import { IEmployeeInfo } from '../../../models/account/profile/employee/employee-info.model';
import { IEmployeeEditInfo } from '../../../models/account/profile/employee/employee-edit-info.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IEmployeeRegister): Observable<IEmployeeProfile> {
    const url = `${this.urlRoot}/account/employee/register`;
    return this.http.post<IEmployeeProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IEmployeeProfile> {
    const url = `${this.urlRoot}/account/employee/profile`;
    return this.http.get<IEmployeeProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IEmployeeInfo> {
    const url = `${this.urlRoot}/account/employee/profile/info`;
    return this.http.get<IEmployeeInfo>(url, this.httpOptions);
  }

  editInfo(data: IEmployeeEditInfo): Observable<IEmployeeInfo> {
    const url = `${this.urlRoot}/account/employee/profile/info`;
    return this.http.post<IEmployeeInfo>(url, data, this.httpOptions);
  }

}
