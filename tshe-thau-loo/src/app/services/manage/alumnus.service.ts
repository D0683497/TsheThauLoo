import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAlumnusManage } from '../../models/manage/alumnus-manage.models';
import { IAlumnus } from '../../models/manage/alumnus.models';

@Injectable({
  providedIn: 'root'
})
export class AlumnusService {

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

  getAlumni(pageIndex: number, pageSize: number): Observable<HttpResponse<IAlumnus[]>> {
    const url = `${this.urlRoot}/alumni?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IAlumnus[]>(url, this.httpResponseOptions);
  }

  getAlumnus(userId: string): Observable<IAlumnus> {
    const url = `${this.urlRoot}/alumni/${userId}`;
    return this.http.get<IAlumnus>(url, this.httpOptions);
  }

  editAlumnus(userId: string, data: IAlumnusManage): Observable<IAlumnus> {
    const url = `${this.urlRoot}/alumni/${userId}`;
    return this.http.post<IAlumnus>(url, data, this.httpOptions);
  }
}
