import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IManager } from '../../models/manage/manager.models';
import { IManagerManage } from '../../models/manage/manager-manage.models';

@Injectable({
  providedIn: 'root'
})
export class ManagerService {

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

  getManagers(pageIndex: number, pageSize: number): Observable<HttpResponse<IManager[]>> {
    const url = `${this.urlRoot}/managers?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IManager[]>(url, this.httpResponseOptions);
  }

  getManager(userId: string): Observable<IManager> {
    const url = `${this.urlRoot}/managers/${userId}`;
    return this.http.get<IManager>(url, this.httpOptions);
  }

  editManager(userId: string, data: IManagerManage): Observable<IManager> {
    const url = `${this.urlRoot}/managers/${userId}`;
    return this.http.post<IManager>(url, data, this.httpOptions);
  }
}
