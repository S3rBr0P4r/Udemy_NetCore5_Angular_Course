import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';
import { PaginatedResult } from '../models/pagination';
import { UserParams } from '../models/userParams';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];


  constructor(private httpClient: HttpClient) { }

  getMembers(userParams: UserParams) {
    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params
      .append('minAge', userParams.minAge.toString())
      .append('maxAge', userParams.maxAge.toString())
      .append('gender', userParams.gender);

    // if (this.members.length > 0) {
    //   return of(this.members);
    // }
    return this.getPaginatedResults<Member[]>(this.baseUrl + 'users', params);
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

  private getPaginatedResults<T>(url, params) {
    const paginatedResults: PaginatedResult<T> = new PaginatedResult<T>();
    return this.httpClient.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResults.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResults.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResults;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params
      .append('pageNumber', pageNumber.toString())
      .append('pageSize', pageSize.toString());

    return params;
  }
}
