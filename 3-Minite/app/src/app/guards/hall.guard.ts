import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ApiService } from '../services/api.service';

@Injectable({
  providedIn: 'root'
})
export class HallGuard implements CanActivate {
  private _ip: string | null = null;
  
  constructor(
    private router: Router,
    private _api: ApiService
  ) {
    this._api.get<{ ip: string }>('ip').subscribe(response => {
      this._ip = response.ip;
    });
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const param = next.queryParams['checkip'];
    if (param === this._ip) {
      return true;
    }

    this.router.navigate(['/error']);
    return false;
  }
}
