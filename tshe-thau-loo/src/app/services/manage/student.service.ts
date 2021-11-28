import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IStudent } from '../../models/manage/student.models';
import { IStudentManage } from '../../models/manage/student-manage.models';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };
  httpResponseOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    observe: 'response' as const
  };

  constructor(private http: HttpClient) { }

  getStudents(pageIndex: number, pageSize: number): Observable<HttpResponse<IStudent[]>> {
    const url = `${this.urlRoot}/students?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IStudent[]>(url, this.httpResponseOptions);
  }

  getStudent(userId: string): Observable<IStudent> {
    const url = `${this.urlRoot}/students/${userId}`;
    return this.http.get<IStudent>(url, this.httpOptions);
  }

  editStudent(userId: string, data: IStudentManage): Observable<IStudent> {
    const url = `${this.urlRoot}/students/${userId}`;
    return this.http.post<IStudent>(url, data, this.httpOptions);
  }
}
