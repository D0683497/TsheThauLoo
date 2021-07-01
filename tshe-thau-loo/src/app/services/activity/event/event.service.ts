import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEvent } from '../../../models/activity/event/event.model';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { IEventCreate } from '../../../models/activity/event/event-create.model';
import { IEventEdit } from '../../../models/activity/event/event-edit.model';

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

}
