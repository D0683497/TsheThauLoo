import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IStudentRegister } from '../../../models/account/register/student-register.model';
import { Observable } from 'rxjs';
import { IStudentProfile } from '../../../models/account/profile/student/student-profile.model';
import { IStudentInfo } from '../../../models/account/profile/student/student-info.model';
import { IStudentEditInfo } from '../../../models/account/profile/student/student-edit-info.model';
import { IStudentVerify } from '../../../models/account/profile/student/student-verify.model';
import { IStudentEditVerify } from '../../../models/account/profile/student/student-edit-verify.model';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IStudentRegister): Observable<IStudentProfile> {
    const url = `${this.urlRoot}/account/student/register`;
    return this.http.post<IStudentProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IStudentProfile> {
    const url = `${this.urlRoot}/account/student/profile`;
    return this.http.get<IStudentProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IStudentInfo> {
    const url = `${this.urlRoot}/account/student/profile/info`;
    return this.http.get<IStudentInfo>(url, this.httpOptions);
  }

  editInfo(data: IStudentEditInfo): Observable<IStudentInfo> {
    const url = `${this.urlRoot}/account/student/profile/info`;
    return this.http.post<IStudentInfo>(url, data, this.httpOptions);
  }

  getVerify(): Observable<IStudentVerify> {
    const url = `${this.urlRoot}/account/student/profile/verify`;
    return this.http.get<IStudentVerify>(url, this.httpOptions);
  }

  editVerify(data: IStudentEditVerify): Observable<IStudentVerify> {
    const url = `${this.urlRoot}/account/student/profile/verify`;
    return this.http.post<IStudentVerify>(url, data, this.httpOptions);
  }

  createVerifyFile(file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/account/student/profile/verify/files`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  editVerifyFile(fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/account/student/profile/verify/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  getVerifyFile(fileId: string): Observable<IDocument> {
    const url = `${this.urlRoot}/account/student/profile/verify/files/${fileId}`;
    return this.http.get<IDocument>(url, this.httpOptions);
  }

  downloadVerifyFile(fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/account/student/profile/verify/files/${fileId}/download`;
    return this.http.get(url, { responseType: 'blob' });
  }

  deleteVerifyFile(fileId: string): Observable<void> {
    const url = `${this.urlRoot}/account/student/profile/verify/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
