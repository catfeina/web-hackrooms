import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-textbox',
  templateUrl: './textbox.component.html',
  styleUrl: './textbox.component.css'
})
export class TextboxComponent {
  @Input() ToolTip: string = '';
  @Input() isPassword: boolean = false;
  @Input() value: string = '';
  @Output() valueChange: EventEmitter<string> = new EventEmitter<string>();

  isFocused: boolean = false;

  onInput(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.value = inputElement.value;
    this.valueChange.emit(this.value);
  }

  onFocus(): void {
    this.isFocused = true;
  }

  onBlur(): void {
    this.isFocused = false;
  }
}
