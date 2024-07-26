import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-arrow',
  templateUrl: './arrow.component.html',
  styleUrl: './arrow.component.css'
})
export class ArrowComponent implements OnInit {
  message: string = '';
  flag: string = '';
  data: any[] = [];

  constructor(
    private _api: ApiService
  ) { }

  ngOnInit(): void {
    this.getFlag();
  }

  getFlag() {
    this._api.getPrivateData('flag').subscribe(
      data => {
        this.message = data.message;
        this.flag = data.flag;
      },
      error => {
        this.message = `Error retrieving data: ${error}`;
        this._api.logout();
      }
    );
  }
}
