import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleService } from '../services/role.service';
import { map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoadGuard implements CanActivate {
  constructor(
    private _route: Router,
    private _role: RoleService
  ) { }
  canActivate(): Observable<boolean> {
    return this._role.IsAuthenticated().pipe(
      take(1), // Toma solo el primer valor emitido
      map(isAuth => {
        if (isAuth) {
          this._route.navigate(['/street']);
          return false;
        } else {
          return true;
        }
      })
    );
  }
}