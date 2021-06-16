import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IExaminerRegister } from '../../../models/account/register/examiner-register.model';
import { Observable } from 'rxjs';
import { IExaminerProfile } from '../../../models/account/profile/examiner/examiner-profile.model';
import { IExaminerInfo } from '../../../models/account/profile/examiner/examiner-info.model';
import { IExaminerEditInfo } from '../../../models/account/profile/examiner/examiner-edit-info.model';

@Injectable({
  providedIn: 'root'
})
export class ExaminerService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IExaminerRegister): Observable<IExaminerProfile> {
    const url = `${this.urlRoot}/account/examiner/register`;
    return this.http.post<IExaminerProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IExaminerProfile> {
    const url = `${this.urlRoot}/account/examiner/profile`;
    return this.http.get<IExaminerProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IExaminerInfo> {
    const url = `${this.urlRoot}/account/examiner/profile/info`;
    return this.http.get<IExaminerInfo>(url, this.httpOptions);
  }

  editInfo(data: IExaminerEditInfo): Observable<IExaminerInfo> {
    const url = `${this.urlRoot}/account/examiner/profile/info`;
    return this.http.post<IExaminerInfo>(url, data, this.httpOptions);
  }

}
