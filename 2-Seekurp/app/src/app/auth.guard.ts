import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const param = next.queryParams['flag110'];

    if (param === '09af8935b75fd7bd40d88d6c8645c99b') {
      return true; // Permite el acceso
    } else {
      // Redirige a la página de inicio o a una página de error
      this.router.navigate(['/error']);
      return false; // Bloquea el acceso
    }
  }
}
