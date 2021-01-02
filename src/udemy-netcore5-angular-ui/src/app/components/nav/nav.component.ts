import { Component, OnInit } from '@angular/core';
import { error } from 'protractor';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}

  constructor(public accounService: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accounService.login(this.model).subscribe(response => {
      console.log(response);
    },
    error => {
      console.log(error)
    })
  }

  logout() {
    this.accounService.logout();
  }
}
