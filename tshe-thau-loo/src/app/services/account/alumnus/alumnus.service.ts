import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAlumnusRegister } from '../../../models/account/register/alumnus-register.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlumnusService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IAlumnusRegister): Observable<void> {
    const url = `${this.urlRoot}/account/alumnus/register`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

}
