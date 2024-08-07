import { Component } from '@angular/core';
import { RoleService } from './services/role.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title: string = 'app';
  IsAuthenticate: boolean = false;

  constructor(
    private _role: RoleService,
    private _route: Router
  ) {
    this._role.IsAuthenticated().subscribe(isAuth => {
      this.IsAuthenticate = isAuth;
      this.title = _role.GetRole();
    });
  }

  Logout() {
    this._role.Logout();
  }

  Back() {
    this._route.navigate(['/street']);
  }
}
