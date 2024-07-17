import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ButtonComponent } from "./components/button/button.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ButtonComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'app';

  constructor(
    private router: Router
  ) {}

  gotoHome() {
    window.location.href = this.router.createUrlTree(['/ocean']).toString();
  }
}
