import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginatedResults: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private httpClient: HttpClient) { }

  getMembers(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params
      .append('pageNumber', page.toString())
      .append('pageSize', itemsPerPage.toString());
    }

    // if (this.members.length > 0) {
    //   return of(this.members);
    // }
    return this.httpClient.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
      map(response => {
        this.paginatedResults.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          this.paginatedResults.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return this.paginatedResults;
      })
      // map(members => {
      //   this.members = members;
      //   return members;
      // })
    );
  }

  getMember(userName: string) {
    const member = this.members.find(m => m.userName === userName);
    if (member !== undefined) {
      return of(member);
    }
    return this.httpClient.get<Member>(this.baseUrl + 'users/' + userName);
  }

  updateMember(member: Member) {
    return this.httpClient.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.httpClient.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.httpClient.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
