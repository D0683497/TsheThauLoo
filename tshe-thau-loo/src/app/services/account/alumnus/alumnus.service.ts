import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAlumnusRegister } from '../../../models/account/register/alumnus-register.model';
import { Observable } from 'rxjs';
import { IAlumnusProfile } from '../../../models/account/profile/alumnus/alumnus-profile.model';
import { IAlumnusInfo } from '../../../models/account/profile/alumnus/alumnus-info.model';
import { IAlumnusEditInfo } from '../../../models/account/profile/alumnus/alumnus-edit-info.model';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';
import { IAlumnusVerify } from '../../../models/account/profile/alumnus/alumnus-verify.model';
import { IAlumnusEditVerify } from '../../../models/account/profile/alumnus/alumnus-edit-verify.model';

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

  register(data: IAlumnusRegister): Observable<IAlumnusProfile> {
    const url = `${this.urlRoot}/account/alumnus/register`;
    return this.http.post<IAlumnusProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IAlumnusProfile> {
    const url = `${this.urlRoot}/account/alumnus/profile`;
    return this.http.get<IAlumnusProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IAlumnusInfo> {
    const url = `${this.urlRoot}/account/alumnus/profile/info`;
    return this.http.get<IAlumnusInfo>(url, this.httpOptions);
  }

  editInfo(data: IAlumnusEditInfo): Observable<IAlumnusInfo> {
    const url = `${this.urlRoot}/account/alumnus/profile/info`;
    return this.http.post<IAlumnusInfo>(url, data, this.httpOptions);
  }

  getVerify(): Observable<IAlumnusVerify> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify`;
    return this.http.get<IAlumnusVerify>(url, this.httpOptions);
  }

  editVerify(data: IAlumnusEditVerify): Observable<IAlumnusVerify> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify`;
    return this.http.post<IAlumnusVerify>(url, data, this.httpOptions);
  }

  createVerifyFile(file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify/files`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  editVerifyFile(fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  getVerifyFile(fileId: string): Observable<IDocument> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify/files/${fileId}`;
    return this.http.get<IDocument>(url, this.httpOptions);
  }

  downloadVerifyFile(fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify/files/${fileId}/download`;
    return this.http.get(url, { responseType: 'blob' });
  }

  deleteVerifyFile(fileId: string): Observable<void> {
    const url = `${this.urlRoot}/account/alumnus/profile/verify/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
