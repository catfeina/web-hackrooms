import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { RoleService } from '../services/role.service';

@Injectable({
  providedIn: 'root'
})
export class RoadGuard implements CanActivate {
  constructor(
    private _route: Router,
    private _role: RoleService
  ) { }

  canActivate() {
    if (this._role.IsAuthenticate()) {
      this._route.navigate(['/street']);
      return false;
    } else {
      return true;
    }
  }
}