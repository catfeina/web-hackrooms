import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Environment } from '../environments/environments';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private urlApi = Environment.apiUrl;

  constructor(
    private _http: HttpClient,
    private cookieService: CookieService
  ) { }

  public authenticate(
    username: string,
    password: string
  ): Observable<any> {
    return this._http.post<any>(`${this.urlApi}/Auth/login`,
      { username, password }, { withCredentials: true }
    )
      .pipe(
        catchError(this.handleError)
      );
  }

  public isAuthenticated(): boolean {
    return this.cookieService.check('.AspNetCore.Identity.Application');
  }

  public logout(): void {
    this._http.post(`${this.urlApi}/Auth/logout`, {}, { withCredentials: true }).subscribe(
      () => {
        console.log('Logout successful');
        this.cookieService.delete('.AspNetCore.Identity.Application');
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

  public getPrivateData(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`, { withCredentials: true })
      .pipe(
        catchError(this.handleError)
      );
  }

  public getPublicData(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`${this.urlApi}/${endpoint}`, { withCredentials: false })
      .pipe(
        catchError(this.handleError)
      );
  }
}