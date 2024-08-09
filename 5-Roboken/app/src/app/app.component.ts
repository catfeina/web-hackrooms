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
    const storedTitle = localStorage.getItem('role');
    this.title = storedTitle ? storedTitle : this.title;

    this._role.IsAuthenticated().subscribe(isAuth => {
      this.IsAuthenticate = isAuth;
      if (isAuth) {
        const role = this._role.GetRole();
        this.title = role ? role : this.title;
      }
    });
  }

  Logout() {
    this._role.Logout();
  }

  Back() {
    this._route.navigate(['/street']);
  }
}
