import { CanActivate, Router } from '@angular/router';
import { map, Observable, take } from 'rxjs';
import { RoleService } from '../services/role.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HouseGuard implements CanActivate {
  constructor(
    private _role: RoleService,
    private _route: Router
  ) { }
  canActivate(): Observable<boolean> {
    return this._role.IsAuthenticated().pipe(
      take(1),
      map(isAuth => {
        if (isAuth) {
          return true;
        } else {
          this._route.navigate(['/road']);
          return false;
        }
      })
    );
  }
}
