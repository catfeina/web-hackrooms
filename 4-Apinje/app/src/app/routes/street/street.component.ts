import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-street',
  templateUrl: './street.component.html',
  styleUrl: './street.component.css'
})
export class StreetComponent implements OnInit {
  constructor(
    private _api: ApiService,
    private _route: Router
  ) { }
  ngOnInit(): void {
    if (this._api.isAuthenticated()) {
      this._route.navigate(['/arrow']);
    }
  }

  _txtUser: string = '';
  _txtPass: string = '';
  message: string = '';

  login() {
    if (this._txtUser == '') {
      this.message = 'Debe ingresar su usuario para continuar';
      return;
    }

    if (this._txtPass == '') {
      this.message = 'Debe ingresar su contraseña para continuar';
      return;
    }

    this.authenticateUser(this._txtUser, this._txtPass);
  }

  authenticateUser(user: string, pass: string) {
    this._api.authenticate(user, pass).subscribe(
      () => {
        this.message = 'Autenticado correctamente';
        this._route.navigate(['/arrow']).then(() => {
          window.location.reload();
        });
      },
      error => {
        this.message = `Authentication error: ${error}`;
      }
    );
  }
}
