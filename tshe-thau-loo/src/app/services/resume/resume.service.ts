import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { IDocument } from '../../models/document/document.model';
import { IDocumentEdit } from '../../models/document/document-edit.model';

@Injectable({
  providedIn: 'root'
})
export class ResumeService {

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

  getResumes(pageIndex: number, pageSize: number, archive: boolean): Observable<HttpResponse<IDocument[]>> {
    let url = `${this.urlRoot}/resumes?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    if (archive) {
      url = `${url}&archive=true`;
    } else {
      url = `${url}&archive=false`;
    }
    return this.http.get<IDocument[]>(url, this.httpResponseOptions);
  }

  getResume(resumeId: string): Observable<Blob> {
    const url = `${this.urlRoot}/resumes/${resumeId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  createResume(file: File): Observable<IDocument> {
    const url = `${this.urlRoot}/resumes`;
    const form = new FormData();
    form.append('type', file.type);
    form.append('name', file.name);
    form.append('fileData', file);
    return this.http.post<IDocument>(url, form);
  }

  editResume(fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/resumes/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  deleteResume(resumeId: string): Observable<void> {
    const url = `${this.urlRoot}/resumes/${resumeId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
