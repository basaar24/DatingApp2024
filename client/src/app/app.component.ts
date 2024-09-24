import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient);
  title = 'Date me';
  users: any;

  ngOnInit(): void {
    this.http.get("https://localhost:5001/api/users").subscribe({
      next: (response) => { this.users = response },
      error: (error) => { console.log(error) },
      complete: () => { console.log("Request completed!") }
    });
  }
}
