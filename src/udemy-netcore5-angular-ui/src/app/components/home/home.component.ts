import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  registerToogle() {
    this.registerMode = !this.registerMode;
  }

  getUsers() {
    this.httpClient.get("http://localhost:5000/users").subscribe(
      response => {
        this.users = response
      },
      error => {
        console.error('There was the following issue ' + error)
      });
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
