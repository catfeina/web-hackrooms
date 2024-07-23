import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {
  @Input() isPrimary: boolean = false;
  @Input() isEnabled: boolean = false;
  @Input() Label: string = '';
  @Output() onClick = new EventEmitter<void>();

  handleClick() {
    if (this.isEnabled) {
      this.onClick.emit();
    }
  }
}
