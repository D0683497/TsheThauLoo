import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICompany } from '../../models/company/company.model';
import { ICompanyCreate } from '../../models/company/company-create.model';
import { ICompanyEdit } from '../../models/company/company-edit.model';
import { IDocument } from '../../models/document/document.model';
import { IIndustrialClassificationCreate } from '../../models/company/industrial-classification-create.model';
import { IIndustrialClassificationEdit } from '../../models/company/industrial-classification-edit.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

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

  getCompanies(pageIndex: number, pageSize: number): Observable<HttpResponse<ICompany[]>> {
    const url = `${this.urlRoot}/companies?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<ICompany[]>(url, this.httpResponseOptions);
  }

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

  getLogo(companyId: string): Observable<Blob> {
    const url = `${this.urlRoot}/companies/${companyId}/logo`;
    return this.http.get(url, { responseType: 'blob' });
  }

  createLogo(companyId: string, file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/companies/${companyId}/logo`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  deleteLogo(companyId: string): Observable<void> {
    const url = `${this.urlRoot}/companies/${companyId}/logo`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  createSIC(companyId: string, data: IIndustrialClassificationCreate): Observable<ICompany> {
    const url = `${this.urlRoot}/companies/${companyId}/sic`;
    return this.http.post<ICompany>(url, data, this.httpOptions);
  }

  editSIC(companyId: string, sicId: string, data: IIndustrialClassificationEdit): Observable<ICompany> {
    const url = `${this.urlRoot}/companies/${companyId}/sic/${sicId}`;
    return this.http.post<ICompany>(url, data, this.httpOptions);
  }

  deleteSIC(companyId: string, sicId: string): Observable<void> {
    const url = `${this.urlRoot}/companies/${companyId}/sic/${sicId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
