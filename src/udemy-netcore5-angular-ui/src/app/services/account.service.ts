import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response) => {
        const mappedUser = <User>response;
        if (mappedUser) {
          localStorage.setItem('user', JSON.stringify(mappedUser));
          this.currentUserSource.next(mappedUser);
          return 'User ' + mappedUser.userName + ' successfully logged in';
        }

        return 'User ' + model.userName + ' was not able to log in';
      })
    )
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        const mappedUser = <User>user;
        if (mappedUser) {
          localStorage.setItem('user', JSON.stringify(mappedUser));
          this.currentUserSource.next(mappedUser);
        }

        return 'User successfully registered';
      })
    )
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
