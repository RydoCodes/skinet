import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {

  error: any;

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation(); // Returns the current Navigation object when the router is navigating,
    this.error = navigation?.extras?.state?.rydoerror;  // being safe by doing null checks.
  }

  ngOnInit(): void {
  }

}
