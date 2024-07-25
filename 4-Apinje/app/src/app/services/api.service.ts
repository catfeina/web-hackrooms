import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Environment } from '../environments/environments';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private urlApi = Environment.apiUrl;
  private urlApiForConstructor = Environment.apiUrlConstructor;

  constructor(
    private _http: HttpClient
  ) { }

  public authenticate(
    username: string,
    password: string
  ): Observable<any> {
    return this._http.post<any>(`${this.urlApi}/Auth/login`, { username, password }, { withCredentials: true })
      .pipe(
        catchError(this.handleError)
      );
  }

  public isAuthenticated(): boolean {
    console.log(document);
    return document.cookie.includes('AspNetCore.Identity.Application');
  }

  public logout(): void {
    // No necesitas eliminar cookies manualmente; una solicitud al endpoint de logout del backend será suficiente
    this._http.post(`${this.urlApi}/Auth/logout`, {}, { withCredentials: true })
      .subscribe(() => {
        console.log('Logout successful');
      }, error => {
        console.error('Logout failed', error);
      });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 401) {
      return throwError('Invalid username or password');
    } else {
      return throwError('An unknown error occurred');
    }
  }

  public getData(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`, { withCredentials: true })
      .pipe(
        catchError(this.handleError)
      );
  }

  public getDataForConstructor(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApiForConstructor}/${endpoint}`, { withCredentials: true })
      .pipe(
        catchError(this.handleError)
      );
  }

  public postData(endpoint: string, data: any): Observable<any> {
    return this._http.post<any>(`${this.urlApi}/${endpoint}`, data, { withCredentials: true })
      .pipe(
        catchError(this.handleError)
      );
  }
}