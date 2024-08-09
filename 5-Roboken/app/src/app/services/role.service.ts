import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { ApiResponse } from '../interfaces/Api';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  constructor(
    private _api: ApiService,
    private _route: Router
  ) { }

  private _isAuthenticated = new BehaviorSubject<boolean>(document.cookie.includes('Patoken'));

  public Login(
    username: string,
    password: string
  ): Observable<any> {
    return this._api.Post("User/Login", { username, password }).pipe(
      tap(response => {
        if (response.success && response.roles) {
          localStorage.setItem('role', response.roles[0]);
          this._isAuthenticated.next(true);
        }
      })
    );
  }

  public Logout(): void {
    this._api.Post<ApiResponse>('User/Logout', {}).subscribe(
      response => {
        if (response.success) {
          this._isAuthenticated.next(false);
          document.cookie = 'Patoken=; Max-Age=-99999999;';
          localStorage.removeItem('name');
          this._route.navigate(['/road']);
        } else {
          console.log('[+] Logout error: ', response.message);
        }
      }, error => {
        console.log('[+] Logout error: ', error);
      }
    );
  }

  public GetRole(): string {
    let role = localStorage.getItem('role');
    if (role) {
      return role;
    } else {
      this.Logout();
      return '';
    }
  }

  public IsAuthenticated(): Observable<boolean> {
    return this._isAuthenticated.asObservable();
  }
}
