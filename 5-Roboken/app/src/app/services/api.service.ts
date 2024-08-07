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
  public Get<T>(
    endpoint: string
  ): Observable<T> {
    return this._http.get<T>(`/api/${endpoint}`, { withCredentials: true }).pipe(catchError(this.HandleError));
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
    let errorMessage = 'Internal Server Error :c'; // Default message

    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error && error.error.message) {
        errorMessage = error.error.message; // Use the message provided by the server
      } else {
        switch (error.status) {
          case 401:
            errorMessage = 'Invalid username or password. :c';
            break;
          case 400:
            errorMessage = 'Bad Request :c';
            break;
        }
      }
    }
    return throwError(errorMessage);
  }
}
