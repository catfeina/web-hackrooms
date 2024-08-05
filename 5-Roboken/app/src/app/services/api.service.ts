import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private _http: HttpClient
  ) { }
  public Get(
    endpoint: string
  ): Observable<any> {
    return this._http.get<any>(`/api/${endpoint}`, { withCredentials: true }).pipe(catchError(this.HandleError));
  }

  public Post<T>(
    endpoint: string,
    data: any
  ): Observable<T> {
    return this._http.post<T>(`/api/${endpoint}`, data, { withCredentials: true }).pipe(
      catchError(this.HandleError)
    );
  }

  private HandleError(error: HttpErrorResponse) {
    switch (error.status) {
      case 401:
        return throwError('Invalid username or password. :c');
      case 400:
        return throwError('Bad Request :c');
      default:
        return throwError('Internal Server Error :c');
    }
  }
}
