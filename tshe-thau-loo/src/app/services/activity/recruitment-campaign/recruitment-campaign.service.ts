import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';
import { IRecruitmentCampaign } from '../../../models/activity/recruitment-campaign/recruitment-campaign.model';
import { IRecruitmentCampaignCreate } from '../../../models/activity/recruitment-campaign/recruitment-campaign-create.model';
import { IRecruitmentCampaignEdit } from '../../../models/activity/recruitment-campaign/recruitment-campaign-edit.model';
import { ICompany } from '../../../models/company/company.model';

@Injectable({
  providedIn: 'root'
})
export class RecruitmentCampaignService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getRecruitmentCampaign(campaignId: string, recruitmentId: string): Observable<IRecruitmentCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}`;
    return this.http.get<IRecruitmentCampaign>(url, this.httpOptions);
  }

  getRecruitmentCampaigns(campaignId: string): Observable<IRecruitmentCampaign[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment`;
    return this.http.get<IRecruitmentCampaign[]>(url, this.httpOptions);
  }

  createRecruitmentCampaign(campaignId: string, data: IRecruitmentCampaignCreate): Observable<IRecruitmentCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment`;
    return this.http.post<IRecruitmentCampaign>(url, data, this.httpOptions);
  }

  editRecruitmentCampaign(campaignId: string, recruitmentId: string, data: IRecruitmentCampaignEdit): Observable<IRecruitmentCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}`;
    return this.http.post<IRecruitmentCampaign>(url, data, this.httpOptions);
  }

  createRecruitmentCampaignFile(campaignId: string, recruitmentId: string, date: File): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/files`;
    const form = new FormData();
    form.append('type', date.type);
    form.append('name', date.name);
    form.append('fileData', date);
    return this.http.post<IDocument>(url, form);
  }

  getRecruitmentCampaignFile(campaignId: string, recruitmentId: string, fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/files/${fileId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  editRecruitmentCampaignFile(campaignId: string, recruitmentId: string, fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  deleteRecruitmentCampaignFile(campaignId: string, recruitmentId: string, fileId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  deleteRecruitmentCampaign(campaignId: string, recruitmentId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  inviteRecruitmentCampaign(campaignId: string, recruitmentId: string, companyId: string): Observable<ICompany> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/recruitment/${recruitmentId}/invite`;
    return this.http.post<ICompany>(url, {companyId}, this.httpOptions);
  }

}
