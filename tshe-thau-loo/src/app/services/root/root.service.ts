import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAbout } from '../../models/about.model';

@Injectable({
  providedIn: 'root'
})
export class RootService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  about(): Observable<IAbout[]> {
    const url = `${this.urlRoot}/about`;
    return this.http.get<IAbout[]>(url, this.httpOptions);
  }

}
