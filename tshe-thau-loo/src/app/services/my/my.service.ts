import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICompany } from '../../models/company/company.model';

@Injectable({
  providedIn: 'root'
})
export class MyService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  company(): Observable<ICompany> {
    const url = `${this.urlRoot}/my/company`;
    return this.http.get<ICompany>(url, this.httpOptions);
  }

}
