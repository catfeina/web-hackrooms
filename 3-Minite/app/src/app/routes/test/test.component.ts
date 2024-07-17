import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent {
  Label: string | null = null;

  constructor(
    private _api: ApiService
  ) {
    this._api.get<any>('beers/3').subscribe(response => {
      this.Label = response;
    });
  }
}
