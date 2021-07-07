import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICompany } from '../../models/company/company.model';
import { IMyEvent } from '../../models/activity/my-event/my-event.model';

@Injectable({
  providedIn: 'root'
})
export class MyService {

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

  company(): Observable<ICompany> {
    const url = `${this.urlRoot}/my/company`;
    return this.http.get<ICompany>(url, this.httpOptions);
  }

  myEvents(pageIndex: number, pageSize: number): Observable<HttpResponse<IMyEvent[]>> {
    const url = `${this.urlRoot}/my/events?pageIndex=${pageIndex}&pageSize=${pageSize}`;
    return this.http.get<IMyEvent[]>(url, this.httpResponseOptions);
  }

  myEvent(eventId: string): Observable<IMyEvent> {
    const url = `${this.urlRoot}/my/events/${eventId}`;
    return this.http.get<IMyEvent>(url, this.httpOptions);
  }

}
