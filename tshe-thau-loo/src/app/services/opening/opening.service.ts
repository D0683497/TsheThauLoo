import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IOpening } from '../../models/opening/opening.model';
import { IOpeningCreate } from '../../models/opening/opening-create.model';
import { IOpeningEdit } from '../../models/opening/opening-edit.model';
import { IFacultyCreate } from '../../models/opening/faculty-create.model';
import { IFacultyEdit } from '../../models/opening/faculty-edit.model';
import { IFaculty } from '../../models/opening/faculty.model';
import { IQualification } from '../../models/opening/qualification.model';
import { IQualificationCreate } from '../../models/opening/qualification-create.model';
import { IQualificationEdit } from '../../models/opening/qualification-edit.model';
import { IDeliveryResume } from '../../models/activity/recruitment-campaign/delivery-resume.model';

@Injectable({
  providedIn: 'root'
})
export class OpeningService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getOpenings(campaignId: string, recruitmentId: string): Observable<IOpening[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings`;
    return this.http.get<IOpening[]>(url, this.httpOptions);
  }

  getOpening(campaignId: string, recruitmentId: string, openingId: string): Observable<IOpening> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}`;
    return this.http.get<IOpening>(url, this.httpOptions);
  }

  createOpening(campaignId: string, recruitmentId: string, data: IOpeningCreate): Observable<IOpening> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings`;
    return this.http.post<IOpening>(url, data, this.httpOptions);
  }

  editOpening(campaignId: string, recruitmentId: string, openingId: string, data: IOpeningEdit): Observable<IOpening> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}`;
    return this.http.post<IOpening>(url, data, this.httpOptions);
  }

  getFaculty(campaignId: string, recruitmentId: string, openingId: string, facultyId: string): Observable<IFaculty> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/faculties/${facultyId}`;
    return this.http.get<IFaculty>(url, this.httpOptions);
  }

  getFaculties(campaignId: string, recruitmentId: string, openingId: string): Observable<IFaculty[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/faculties`;
    return this.http.get<IFaculty[]>(url, this.httpOptions);
  }

  createFaculty(campaignId: string, recruitmentId: string, openingId: string, data: IFacultyCreate): Observable<IFaculty> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/faculties`;
    return this.http.post<IFaculty>(url, data, this.httpOptions);
  }

  editFaculty(campaignId: string, recruitmentId: string, openingId: string, facultyId: string, data: IFacultyEdit): Observable<IFaculty> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/faculties/${facultyId}`;
    return this.http.post<IFaculty>(url, data, this.httpOptions);
  }

  deleteFaculty(campaignId: string, recruitmentId: string, openingId: string, facultyId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/faculties/${facultyId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  getQualification(campaignId: string, recruitmentId: string, openingId: string, qualificationId: string): Observable<IQualification> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
                `/qualifications/${qualificationId}`;
    return this.http.get<IQualification>(url, this.httpOptions);
  }

  getQualifications(campaignId: string, recruitmentId: string, openingId: string): Observable<IQualification[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/qualifications`;
    return this.http.get<IQualification[]>(url, this.httpOptions);
  }

  createQualification(campaignId: string, recruitmentId: string, openingId: string,
                      data: IQualificationCreate): Observable<IQualification> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}/qualifications`;
    return this.http.post<IQualification>(url, data, this.httpOptions);
  }

  editQualification(campaignId: string, recruitmentId: string, openingId: string,
                    qualificationId: string, data: IQualificationEdit): Observable<IQualification> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
                `/qualifications/${qualificationId}`;
    return this.http.post<IQualification>(url, data, this.httpOptions);
  }

  deleteQualification(campaignId: string, recruitmentId: string, openingId: string, qualificationId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
      `/qualifications/${qualificationId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  deliveryResume(campaignId: string, recruitmentId: string, openingId: string, resumeId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
      `/delivery`;
    return this.http.post<void>(url, {resumeId}, this.httpOptions);
  }

  deliveryResumeList(campaignId: string, recruitmentId: string, openingId: string): Observable<IDeliveryResume[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
      `/delivery`;
    return this.http.get<IDeliveryResume[]>(url, this.httpOptions);
  }

  deliveryResumeDownload(campaignId: string, recruitmentId: string, openingId: string, resumeId: string): Observable<Blob> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/openings/${openingId}` +
      `/delivery/${resumeId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

}
