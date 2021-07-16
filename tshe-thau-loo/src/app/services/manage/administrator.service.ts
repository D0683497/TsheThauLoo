import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAdministrator } from '../../models/manage/administrator.models';
import { IAdministratorManage } from '../../models/manage/administrator-manage.models';

@Injectable({
  providedIn: 'root'
})
export class AdministratorService {

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

  getAdministrators(pageIndex: number, pageSize: number): Observable<HttpResponse<IAdministrator[]>> {
    const url = `${this.urlRoot}/administrators?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IAdministrator[]>(url, this.httpResponseOptions);
  }

  getAdministrator(userId: string): Observable<IAdministrator> {
    const url = `${this.urlRoot}/administrators/${userId}`;
    return this.http.get<IAdministrator>(url, this.httpOptions);
  }

  editAdministrator(userId: string, data: IAdministratorManage): Observable<IAdministrator> {
    const url = `${this.urlRoot}/administrators/${userId}`;
    return this.http.post<IAdministrator>(url, data, this.httpOptions);
  }

}
