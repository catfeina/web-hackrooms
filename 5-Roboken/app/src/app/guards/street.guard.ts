import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleService } from '../services/role.service';
import { map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StreetGuard implements CanActivate {
  constructor(
    private _role: RoleService,
    private _routes: Router
  ) { }
  canActivate(): Observable<boolean> {
    return this._role.IsAuthenticated().pipe(
      take(1), // Toma solo el primer valor emitido
      map(isAuth => {
        if (isAuth) {
          return true;
        } else {
          this._routes.navigate(['/road']);
          return false;
        }
      })
    );
  }
}
