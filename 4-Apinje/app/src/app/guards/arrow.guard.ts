import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ApiService } from '../services/api.service';

@Injectable({
  providedIn: 'root'
})
export class ArrowGuard implements CanActivate {

  constructor(
    private router: Router,
    private apiService: ApiService
  ) { }

  canActivate(): boolean {
    if (this.apiService.isAuthenticated()) {
      return true;
    } else {
      this.router.navigate(['/poem']);
      return false;
    }
  }
}
