import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEventCreate } from '../../../models/activity/event/event-create.model';

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

  createEvent(data: IEventCreate): Observable<void> {
    const url = `${this.urlRoot}/events`;
    return this.http.post<void>(url, data, this.httpOptions);
  }

}
