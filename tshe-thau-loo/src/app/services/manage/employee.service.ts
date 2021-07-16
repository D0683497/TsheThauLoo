import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEmployee } from '../../models/manage/employee.models';
import { IEmployeeManage } from '../../models/manage/employee-manage.models';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

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

  getEmployees(pageIndex: number, pageSize: number): Observable<HttpResponse<IEmployee[]>> {
    const url = `${this.urlRoot}/employees?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IEmployee[]>(url, this.httpResponseOptions);
  }

  getEmployee(userId: string): Observable<IEmployee> {
    const url = `${this.urlRoot}/employees/${userId}`;
    return this.http.get<IEmployee>(url, this.httpOptions);
  }

  editEmployee(userId: string, data: IEmployeeManage): Observable<IEmployee> {
    const url = `${this.urlRoot}/employees/${userId}`;
    return this.http.post<IEmployee>(url, data, this.httpOptions);
  }
}
