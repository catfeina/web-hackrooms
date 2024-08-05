import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-textbox',
  templateUrl: './textbox.component.html',
  styleUrl: './textbox.component.css'
})
export class TextboxComponent {
  @Input() ToolTip: string = '';
  @Input() IsPassword: boolean = false;
  @Input() Value: string = '';
  @Output() ValueChange: EventEmitter<string> = new EventEmitter<string>();

  isFocused: boolean = false;

  onInput(event: Event): void {
    const text = event.target as HTMLInputElement;

    this.Value = text.value;
    this.ValueChange.emit(this.Value);
  }

  onFocus(): void {
    this.isFocused = true;
  }

  onBlur(): void {
    this.isFocused = false;
  }
}
