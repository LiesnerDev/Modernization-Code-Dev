import { Component, Input } from '@angular/core';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrls: ['./custom-input.component.scss']
})
export class CustomInputComponent {
  @Input() id!: string;
  @Input() label!: string;
  @Input() placeholder!: string;
  @Input() name!: string;
  @Input() required: boolean = false;
  @Input() pattern: string = '';
  @Input() maxlength?: number;
  @Input() errorMessage: string = '';
}