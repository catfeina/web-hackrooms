import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  constructor(
    private _api: ApiService,
    private _route: Router
  ) { }

  private _role: string = '';

  public Login(
    username: string,
    password: string
  ): Observable<any> {
    return this._api.Post("User/Login", { username, password }).pipe(
      tap(response => {
        if (response.success && response.roles) {
          this._role = response.roles[0];
        }
      })
    );
  }

  public Logout(): void {
    this._api.Post('User/Logout', {}).subscribe(() => {
      console.log('Logout succesful!');
      document.cookie = 'Patoken=; Max-Age=-99999999;';
      this._route.navigate(['/road']);
    });
  }

  public GetRole() {
    return this._role;
  }

  public IsAuthenticate(): boolean {
    return document.cookie.includes('Patoken');
  }
}
