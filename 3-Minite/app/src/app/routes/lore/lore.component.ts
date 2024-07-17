import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { PlatformService } from '../../services/platform.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-lore',
  standalone: true,
  imports: [],
  templateUrl: './lore.component.html',
  styleUrls: ['./lore.component.css']
})
export class LoreComponent implements OnInit {
  Label: string | null = null;
  responseData: any;

  constructor(
    private _api: ApiService,
    private _route: ActivatedRoute,
    private _browser: PlatformService,
    private _goto: Router
  ) {}

  ngOnInit(): void {
    let a = this._route.snapshot.queryParamMap.get('a');
    let b = this._route.snapshot.queryParamMap.get('b');
    let c = this._route.snapshot.queryParamMap.get('c');

    if (a && b && c) {
      if (a === 'flag110') {
        window.location.href = this._goto.createUrlTree(['/ocean']).toString();
      }
      
      this._api.get<{ msg: string }>(`nothinghere/${a}/${b}/${c}`).subscribe(response => {
        this.Label = response.msg;
      });
    }

    let create = this._route.snapshot.queryParamMap.get('create');
    if (create) {
      this.createDataInStorage();
    }
  }

  private createDataInStorage(): void {
    const data = {
      a: 'flag110',
      b: 'flag6',
      c: 'creature'
    };

    if (this._browser.isBrowser()) {
      localStorage.setItem('data', JSON.stringify(data));
    }

    console.log('Data created in localStorage:', data);
  }
}
