import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  loggedIn: boolean = false;

  constructor(private accounService: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accounService.login(this.model).subscribe(response => {
      console.log(response);
      this.loggedIn = true;
    },
    error => {
      console.log(error)
    })
  }

  logout() {
    this.loggedIn = false;
  }
}
