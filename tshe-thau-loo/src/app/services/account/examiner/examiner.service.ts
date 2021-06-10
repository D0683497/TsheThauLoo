import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IExaminerRegister } from '../../../models/account/register/examiner-register.model';
import { Observable } from 'rxjs';
import { IExaminerProfile } from '../../../models/account/profile/examiner-profile.model';

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

}
