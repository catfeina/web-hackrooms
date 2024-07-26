import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-poem',
  templateUrl: './poem.component.html',
  styleUrl: './poem.component.css'
})
export class PoemComponent implements OnInit {
  constructor(
    private _api: ApiService,
    private _me: ActivatedRoute,
    private _route: Router
  ) { }

  data: any[] = [];

  ngOnInit(): void {
    this._me.params.subscribe(params => {
      const id = params["id"];
      console.log('[+] pato')

      if (id) {
        this.getPoem(id);
      } else {
        console.log('redirigiendo...');
        this._route.navigate(['/poem/', '0']);
      }
    });
  }

  getPoem(id: string) {
    this._api.getPublicData(`Paragraph/${id}`).subscribe(
      response => {
        this.data = response;
      },
      error => {
        console.log(error);
      }
    );
  }
}
