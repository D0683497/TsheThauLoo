import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAlumnusRegister } from '../../../models/account/register/alumnus-register.model';
import { Observable } from 'rxjs';
import { IAlumnusProfile } from '../../../models/account/profile/alumnus/alumnus-profile.model';
import { IAlumnusInfo } from '../../../models/account/profile/alumnus/alumnus-info.model';
import { IAlumnusEditInfo } from '../../../models/account/profile/alumnus/alumnus-edit-info.model';

@Injectable({
  providedIn: 'root'
})
export class AlumnusService {

  urlRoot = environment.apiUrl;
  httpOptions = {
    // eslint-disable-next-line @typescript-eslint/naming-convention
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  register(data: IAlumnusRegister): Observable<IAlumnusProfile> {
    const url = `${this.urlRoot}/account/alumnus/register`;
    return this.http.post<IAlumnusProfile>(url, data, this.httpOptions);
  }

  getProfile(): Observable<IAlumnusProfile> {
    const url = `${this.urlRoot}/account/alumnus/profile`;
    return this.http.get<IAlumnusProfile>(url, this.httpOptions);
  }

  getInfo(): Observable<IAlumnusInfo> {
    const url = `${this.urlRoot}/account/alumnus/profile/info`;
    return this.http.get<IAlumnusInfo>(url, this.httpOptions);
  }

  editInfo(data: IAlumnusEditInfo): Observable<IAlumnusInfo> {
    const url = `${this.urlRoot}/account/alumnus/profile/info`;
    return this.http.post<IAlumnusInfo>(url, data, this.httpOptions);
  }

}
