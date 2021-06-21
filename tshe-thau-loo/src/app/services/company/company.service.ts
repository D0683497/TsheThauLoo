import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICompany } from '../../models/company/company.model';
import { ICompanyCreate } from '../../models/company/company-create.model';
import { ICompanyEdit } from '../../models/company/company-edit.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getCompany(companyId: string): Observable<ICompany> {
    const url = `${this.urlRoot}/companies/${companyId}`;
    return this.http.get<ICompany>(url, this.httpOptions);
  }

  create(data: ICompanyCreate): Observable<ICompany> {
    const url = `${this.urlRoot}/companies`;
    return this.http.post<ICompany>(url, data, this.httpOptions);
  }

  edit(companyId: string, data: ICompanyEdit): Observable<ICompany> {
    const url = `${this.urlRoot}/companies/${companyId}`;
    return this.http.post<ICompany>(url, data, this.httpOptions);
  }

}
