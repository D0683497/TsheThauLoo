import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IExaminer } from '../../models/manage/examiner.models';
import { IExaminerManage } from '../../models/manage/examiner-manage.models';

@Injectable({
  providedIn: 'root'
})
export class ExaminerService {

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

  getExaminers(pageIndex: number, pageSize: number): Observable<HttpResponse<IExaminer[]>> {
    const url = `${this.urlRoot}/examiners?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IExaminer[]>(url, this.httpResponseOptions);
  }

  getExaminer(userId: string): Observable<IExaminer> {
    const url = `${this.urlRoot}/examiners/${userId}`;
    return this.http.get<IExaminer>(url, this.httpOptions);
  }

  editExaminer(userId: string, data: IExaminerManage): Observable<IExaminer> {
    const url = `${this.urlRoot}/examiners/${userId}`;
    return this.http.post<IExaminer>(url, data, this.httpOptions);
  }
}
