import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { ApiService } from '../../services/api.service';
import { ApiResponse } from '../../interfaces/api';

@Component({
  selector: 'app-street',
  templateUrl: './street.component.html',
  styleUrl: './street.component.css'
})
export class StreetComponent {
  constructor(
    private _role: RoleService,
    private _api: ApiService
  ) { }

  Logout() {
    this._role.Logout();
  }

  txtTitle: string = '';
  txtDescription: string = '';
  message: string = '';

  AddTask() {
    const title = this.txtTitle;
    const description = this.txtDescription;

    if (title == '') {
      this.message = 'Debe ingresar un título';
      return;
    }

    if (description == '') {
      this.message = 'Debe ingresar una descripción';
      return;
    }

    this._api.Post<ApiResponse>('Task/Create', { title, description }).subscribe(response => {
      this.message = response.message;
    });
  }
}
