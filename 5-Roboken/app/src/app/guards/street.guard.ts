import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleService } from '../services/role.service';

@Injectable({
  providedIn: 'root'
})
export class StreetGuard implements CanActivate {
  constructor(
    private _role: RoleService,
    private _rutes: Router
  ) { }
  canActivate(): boolean {
    if (this._role.IsAuthenticate()) {
      return true;
    } else {
      this._rutes.navigate(['/road']);
      return false;
    }
  }
}
