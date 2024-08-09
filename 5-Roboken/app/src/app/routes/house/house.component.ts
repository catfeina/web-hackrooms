import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { PoemResponse } from '../../interfaces/Poem';
import { ApiResponse } from '../../interfaces/Api';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-house',
  templateUrl: './house.component.html',
  styleUrl: './house.component.css'
})
export class HouseComponent {
  constructor(
    private _api: ApiService,
    private _role: RoleService
  ) { }

  txtTitle: string = '';
  lblMessage: string = '';
  Poems: PoemResponse[] = [];

  FindByTitle() {
    this.lblMessage = '';

    if (this.txtTitle == '') {
      this.lblMessage = 'Debe ingresar un título para buscar';
      return;
    }

    this._api.Get<PoemResponse[]>(`Poem/${this.txtTitle}`).subscribe(
      response => {
        this.Poems = response;
      }, error => {
        this.lblMessage = error;
      }
    );
  }

  GetFlag() {
    let endpoint;
    switch (this._role.GetRole()) {
      case 'Lvl1':
        endpoint = '/v1';
        break;
      case 'Lvl2':
        endpoint = '/v2';
        break;
      default:
        endpoint = '';
        break;
    }

    this.lblMessage = '';

    if (this.txtTitle == '') {
      this.lblMessage = 'Debe adjuntar un valor para el parámetro';
      return;
    }

    this._api.Get<ApiResponse>(`Flag${endpoint}/${this.txtTitle}`).subscribe(
      response => {
        this.lblMessage = response.message;
      }, error => {
        this.lblMessage = error;
      }
    );
  }
}
