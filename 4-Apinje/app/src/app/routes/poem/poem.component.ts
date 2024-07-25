import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-poem',
  templateUrl: './poem.component.html',
  styleUrl: './poem.component.css'
})
export class PoemComponent implements OnInit {
  constructor(
    private _api: ApiService
  ) { }

  data: any[] = [];

  ngOnInit(): void {
    this.getPoem();
  }

  getPoem() {
    this._api.getDataForConstructor('Paragraph/0').subscribe(
      response => {
        this.data = response;
      },
      error => {
        console.log(error);
      }
    );
  }
}
