import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-street',
  templateUrl: './street.component.html',
  styleUrl: './street.component.css'
})
export class StreetComponent implements OnInit {
  constructor(
    private _api: ApiService
  ) { }
  ngOnInit(): void {
    this.FullData();
  }

  data: any[] = [];

  FullData() {
    this._api.getDataForConstructor('User').subscribe(
      response => {
        this.data = response;
      },
      error => {
        console.log(error);
      }
    );
  }

  GetUser() {
    this._api.getData('User/1').subscribe(
      user => {
        this.data = user;
      },
      error => {
        console.log(error);
      }
    );
  }

  authenticateUser() {
    this._api.authenticate('test', 'password').subscribe(
      response => {
        localStorage.setItem('authToken', response.token);
        console.log('Autenticado correctamente');
      },
      error => {
        console.error('Authentication error:', error);
      }
    );
  }
}
