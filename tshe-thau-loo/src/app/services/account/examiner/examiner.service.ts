import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IExaminerRegister } from '../../../models/account/register/examiner-register.model';
import { Observable } from 'rxjs';

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

  register(data: IExaminerRegister): Observable<void> {
    const url = `${this.urlRoot}/account/examiner/register`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

}
