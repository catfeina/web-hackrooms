import { Component } from '@angular/core';
import { ButtonComponent } from '../../components/button/button.component';
import { Router } from '@angular/router';
import { PlatformService } from '../../services/platform.service';

@Component({
  selector: 'app-hall',
  standalone: true,
  imports: [ButtonComponent],
  templateUrl: './hall.component.html',
  styleUrl: './hall.component.css'
})
export class HallComponent {
  constructor(
    private router: Router,
    private _browser: PlatformService
  ) {}

  goToLore() {
    if (this._browser.isBrowser()) {
      const storedData = localStorage.getItem('data');

      if (storedData) {
        const jsonData = JSON.parse(storedData);

        window.location.href = this.router.createUrlTree(['/lore'], {queryParams: {
          a: jsonData.a,
          b: jsonData.b,
          c: jsonData.c
        }}).toString();
      } else {
        this.router.navigate(['/lore'], {queryParams: {
          create: 'true'
        }}).toString();  
      }
    } else {
      this.router.navigate(['/lore'], {queryParams: {
        create: 'true'
      }}).toString();
    }
  }
}
