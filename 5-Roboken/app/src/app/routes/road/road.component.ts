import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-road',
  templateUrl: './road.component.html',
  styleUrl: './road.component.css'
})
export class RoadComponent {
  constructor(
    private _role: RoleService,
    private _route: Router
  ) { }

  _txtUser: string = '';
  _txtPass: string = '';
  _message: string = '';

  Login() {
    if (this._txtUser == '') {
      this._message = 'Debe ingresar un usuario para continuar';
      return;
    }

    if (this._txtPass == '') {
      this._message = 'Debe ingresar la contraseña para continuar';
      return;
    }

    this._role.Login(this._txtUser, this._txtPass).subscribe(
      () => {
        this._message = 'Login exitoso :3'
        this._route.navigate(['/street']);
      },
      error => {
        this._message = error;
      }
    );
  }
}
