import { HttpClient } from '@angular/common/http';

import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../models/message';
import { getPaginatedResults, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getMessages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);
    return getPaginatedResults<Message[]>(this.baseUrl + 'messages', params, this.httpClient);
  }

  getMessageThread(userName: string) {
    return this.httpClient.get<Message[]>(this.baseUrl + 'messages/thread/' + userName);
  }
}
