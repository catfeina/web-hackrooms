import { Component } from '@angular/core';
import { HiddenButtonComponent } from "../hidden-button/hidden-button.component";

@Component({
    selector: 'app-pipe',
    standalone: true,
    templateUrl: './pipe.component.html',
    styleUrl: './pipe.component.css',
    imports: [HiddenButtonComponent]
})
export class PipeComponent {
  
}
