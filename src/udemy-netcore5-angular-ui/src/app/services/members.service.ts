import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  httpOptions = new HttpHeaders({
    'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('user'))?.token
  });

  getMembers() {
    return this.httpClient.get<Member[]>(this.baseUrl + 'users', { headers: this.httpOptions });
  }

  getMember(username: string) {
    return this.httpClient.get<Member>(this.baseUrl + 'users/' + username, { headers: this.httpOptions });
  }
}
