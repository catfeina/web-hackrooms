import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
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

  public authenticate(username: string, password: string): Observable<any> {
    return this._http.post<any>(`${this.urlApi}/Auth/login`, { username, password })
      .pipe(
        catchError(error => {
          if (error.status === 401) {
            return throwError('Invalid username or password');
          } else {
            return throwError('An unknown error occurred');
          }
        })
      );
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('authToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  public getData(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`);
  }

  public getDataAuth(
    endpoint: string
  ) {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`, { headers: this.getAuthHeaders() });
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
