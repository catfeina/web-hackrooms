import { Component } from '@angular/core';
import { ButtonComponent } from "../../components/button/button.component";
import { ApiService } from '../../services/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ocean',
  standalone: true,
  imports: [ButtonComponent],
  templateUrl: './ocean.component.html',
  styleUrl: './ocean.component.css'
})
export class OceanComponent {
  message: string | null = null;

  constructor(
    private _api: ApiService,
    private router: Router
  ) {
    this._api.get<{ message: string }>('test').subscribe(response => {
      this.message = response.message;
    });
  }

  inspectHall() {
    window.location.href = this.router.createUrlTree(['/hall'], {queryParams: { checkip: '192.168.0.15' }}).toString();
  }

  inspectLore() {
    window.location.href = this.router.createUrlTree(['/lore']).toString();
  }
}
