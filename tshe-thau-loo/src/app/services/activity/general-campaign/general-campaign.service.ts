import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';
import { IGeneralCampaign } from '../../../models/activity/general-campaign/general-campaign.model';
import { IGeneralCampaignCreate } from '../../../models/activity/general-campaign/general-campaign-create.model';
import { IGeneralCampaignEdit } from '../../../models/activity/general-campaign/general-campaign-edit.model';
import { ICompany } from '../../../models/company/company.model';

@Injectable({
  providedIn: 'root'
})
export class GeneralCampaignService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getGeneralCampaign(campaignId: string, generalId: string): Observable<IGeneralCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}`;
    return this.http.get<IGeneralCampaign>(url, this.httpOptions);
  }

  getGeneralCampaigns(campaignId: string): Observable<IGeneralCampaign[]> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general`;
    return this.http.get<IGeneralCampaign[]>(url, this.httpOptions);
  }

  createGeneralCampaign(campaignId: string, data: IGeneralCampaignCreate): Observable<IGeneralCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general`;
    return this.http.post<IGeneralCampaign>(url, data, this.httpOptions);
  }

  editGeneralCampaign(campaignId: string, generalId: string, data: IGeneralCampaignEdit): Observable<IGeneralCampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}`;
    return this.http.post<IGeneralCampaign>(url, data, this.httpOptions);
  }

  createGeneralCampaignFile(campaignId: string, generalId: string, date: File): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/files`;
    const form = new FormData();
    form.append('type', date.type);
    form.append('name', date.name);
    form.append('fileData', date);
    return this.http.post<IDocument>(url, form);
  }

  getGeneralCampaignFile(campaignId: string, generalId: string, fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/files/${fileId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  editGeneralCampaignFile(campaignId: string, generalId: string, fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  deleteGeneralCampaignFile(campaignId: string, generalId: string, fileId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  deleteGeneralCampaign(campaignId: string, generalId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  signUpGeneralCampaign(campaignId: string, generalId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/sign-up`;
    return this.http.post<void>(url, this.httpOptions);
  }

  signInGeneralCampaign(campaignId: string, generalId: string, userId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/sign-in`;
    return this.http.post<void>(url, {userId}, this.httpOptions);
  }

  participateGeneralCampaign(campaignId: string, generalId: string,
                             data: {name: string; contactPhone: string; remark: string}): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/participate`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

  inviteGeneralCampaign(campaignId: string, generalId: string, companyId: string): Observable<ICompany> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/general/${generalId}/invite`;
    return this.http.post<ICompany>(url, {companyId}, this.httpOptions);
  }

}
