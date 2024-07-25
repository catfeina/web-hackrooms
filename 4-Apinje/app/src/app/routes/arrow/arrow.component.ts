import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-arrow',
  templateUrl: './arrow.component.html',
  styleUrl: './arrow.component.css'
})
export class ArrowComponent implements OnInit {
  data: any[] = [];

  constructor(
    private _api: ApiService
  ) { }
  ngOnInit(): void {
    this.getFlag();
  }

  getFlag() {
    this._api.getData('flag').subscribe(
      data => {
        this.data = data;
        console.log('Data retrieved successfully');
      },
      error => {
        console.log('Error retrieving data', error);
      }
    );
  }
}
