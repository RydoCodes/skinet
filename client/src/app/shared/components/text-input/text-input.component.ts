import { Component, ElementRef, Input, OnInit, Self, ViewChild } from '@angular/core';
import { async } from '@angular/core/testing';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements OnInit, ControlValueAccessor {

  @ViewChild('input', {static: true}) input: ElementRef;
  @Input() type = 'text';
  @Input() label: string;

  // @Self : Not look for any other shared dependency of NgControl that is already in use
  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this; // Gives access to our control directive inside our component
  }

  ngOnInit(): void {
    const control = this.controlDir.control; // Get current control
    const validators = control.validator ? [control.validator] : []; // check what validators have been set on this particular control.
    const asyncValidators = control.asyncValidator ? [control.asyncValidator] : [];

    control.setValidators(validators); // Synchronous Validators are set.
    control.setAsyncValidators(asyncValidators); // ASynchronous Validators are set.
    control.updateValueAndValidity(); // validate our form on Initialisation.
  }

  onChange(event): any {}

  onTouched(): any {}

  // obj is what we will write on the inout field whih gets assigned to the input's value
  writeValue(obj: any): void {
    // input is of type #input placed on the input element in text component html
    this.input.nativeElement.value = obj || '';
  }
  // Just implementing. No real use actually
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  // Just implementing. No real use actually
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

}
