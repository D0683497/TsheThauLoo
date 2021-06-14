import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IStudentRegister } from '../../../models/account/register/student-register.model';
import { Observable } from 'rxjs';
import { IStudentProfile } from '../../../models/account/profile/student/student-profile.model';
import { IStudentInfo } from '../../../models/account/profile/student/student-info.model';
import { IStudentEditInfo } from '../../../models/account/profile/student/student-edit-info';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IStudentRegister): Observable<IStudentProfile> {
    const url = `${this.urlRoot}/account/student/register`;
    return this.http.post<IStudentProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IStudentProfile> {
    const url = `${this.urlRoot}/account/student/profile`;
    return this.http.get<IStudentProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IStudentInfo> {
    const url = `${this.urlRoot}/account/student/profile/info`;
    return this.http.get<IStudentInfo>(url, this.httpOptions);
  }

  editInfo(data: IStudentEditInfo): Observable<IStudentInfo> {
    const url = `${this.urlRoot}/account/student/profile/info`;
    return this.http.post<IStudentInfo>(url, data, this.httpOptions);
  }

}
