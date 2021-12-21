import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {

  busyRequestCount = 0;

  constructor(private spinnerService: NgxSpinnerService) { }

  // responsible for showing the loading indicators
  busy(): void {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'timer',
      bdColor: 'rgba(255,255,255,0.7)',
      color: '#333333'
    });
  }

  // responsible for hiding the loading indicators
  idle(): void {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0)
    {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }

}
