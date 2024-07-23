import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private urlApi = Environment.apiUrl;
  private urlApiForConstructor = Environment.apiUrlConstructor;

  constructor(
    private _http: HttpClient
  ) { }

  public getData(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`);
  }

  public getDataForConstructor(
    endpoint: string
  ) {
    return this._http.get<any>(`${this.urlApiForConstructor}/${endpoint}`);
  }

  public postData(endpoint: string, data: any): Observable<any> {
    return this._http.post<any>(`${this.urlApi}/${endpoint}`, data);
  }
}
