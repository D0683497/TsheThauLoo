import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';
import { ICampaign } from '../../../models/activity/campaign/campaign.model';
import { ICampaignCreate } from '../../../models/activity/campaign/campaign-create.model';
import { ICampaignEdit } from '../../../models/activity/campaign/campaign-edit.model';

@Injectable({
  providedIn: 'root'
})
export class CampaignService {

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

  getCampaign(campaignId: string): Observable<ICampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}`;
    return this.http.get<ICampaign>(url, this.httpOptions);
  }

  getCampaigns(pageIndex: number, pageSize: number, status: ActivityStatus): Observable<HttpResponse<ICampaign[]>> {
    const url = `${this.urlRoot}/campaigns?pageIndex=${pageIndex}&pageSize=${pageSize}&status=${status}`;
    return this.http.get<ICampaign[]>(url, this.httpResponseOptions);
  }

  createCampaign(data: ICampaignCreate): Observable<ICampaign> {
    const url = `${this.urlRoot}/campaigns`;
    return this.http.post<ICampaign>(url, data, this.httpOptions);
  }

  editCampaign(campaignId: string, data: ICampaignEdit): Observable<ICampaign> {
    const url = `${this.urlRoot}/campaigns/${campaignId}`;
    return this.http.post<ICampaign>(url, data, this.httpOptions);
  }

  createCampaignFile(campaignId: string, date: File): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/files`;
    const form = new FormData();
    form.append('type', date.type);
    form.append('name', date.name);
    form.append('fileData', date);
    return this.http.post<IDocument>(url, form);
  }

  getCampaignFile(campaignId: string, fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/files/${fileId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  editCampaignFile(campaignId: string, fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  deleteCampaignFile(campaignId: string, fileId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  deleteCampaign(campaignId: string): Observable<void> {
    const url = `${this.urlRoot}/campaigns/${campaignId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

}
