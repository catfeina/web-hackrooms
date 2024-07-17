import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl: string = Environment.apiUrl;

  constructor(private _http: HttpClient) {}

  get<T>(endpoint: string): Observable<T> {
    return this._http.get<T>(`${this.apiUrl}/${endpoint}`);
  }
}