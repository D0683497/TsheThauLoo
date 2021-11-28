import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEvent } from '../../../models/activity/event/event.model';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { IEventCreate } from '../../../models/activity/event/event-create.model';
import { IEventEdit } from '../../../models/activity/event/event-edit.model';
import { IDocument } from '../../../models/document/document.model';
import { IDocumentEdit } from '../../../models/document/document-edit.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {

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

  getEvent(eventId: string): Observable<IEvent> {
    const url = `${this.urlRoot}/events/${eventId}`;
    return this.http.get<IEvent>(url, this.httpOptions);
  }

  getEvents(pageIndex: number, pageSize: number, status: ActivityStatus): Observable<HttpResponse<IEvent[]>> {
    const url = `${this.urlRoot}/events?pageIndex=${pageIndex}&pageSize=${pageSize}&status=${status}`;
    return this.http.get<IEvent[]>(url, this.httpResponseOptions);
  }

  createEvent(data: IEventCreate): Observable<IEvent> {
    const url = `${this.urlRoot}/events`;
    return this.http.post<IEvent>(url, data, this.httpOptions);
  }

  editEvent(eventId: string, data: IEventEdit): Observable<IEvent> {
    const url = `${this.urlRoot}/events/${eventId}`;
    return this.http.post<IEvent>(url, data, this.httpOptions);
  }

  createEventFile(eventId: string, date: File): Observable<IDocument> {
    const url = `${this.urlRoot}/events/${eventId}/files`;
    const form = new FormData();
    form.append('type', date.type);
    form.append('name', date.name);
    form.append('fileData', date);
    return this.http.post<IDocument>(url, form);
  }

  getEventFile(eventId: string, fileId: string): Observable<Blob> {
    const url = `${this.urlRoot}/events/${eventId}/files/${fileId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  editEventFile(eventId: string, fileId: string, data: IDocumentEdit): Observable<IDocument> {
    const url = `${this.urlRoot}/events/${eventId}/files/${fileId}`;
    return this.http.post<IDocument>(url, data, this.httpOptions);
  }

  deleteEventFile(eventId: string, fileId: string): Observable<void> {
    const url = `${this.urlRoot}/events/${eventId}/files/${fileId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  deleteEvent(eventId: string): Observable<void> {
    const url = `${this.urlRoot}/events/${eventId}`;
    return this.http.delete<void>(url, this.httpOptions);
  }

  signUpEvent(eventId: string): Observable<void> {
    const url = `${this.urlRoot}/events/${eventId}/sign-up`;
    return this.http.post<void>(url, this.httpOptions);
  }

  signInEvent(eventId: string, userId: string): Observable<void> {
    const url = `${this.urlRoot}/events/${eventId}/sign-in`;
    return this.http.post<void>(url, {userId}, this.httpOptions);
  }

  participateEvent(eventId: string, data: {name: string; contactPhone: string; remark: string}): Observable<void> {
    const url = `${this.urlRoot}/events/${eventId}/participate`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

}
