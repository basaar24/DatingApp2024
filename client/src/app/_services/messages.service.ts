import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { setPaginationHeaders, setPaginationResponse } from './paginationHelper';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append("Container", container.toLocaleLowerCase());

    return this.http.get<Message[]>(this.baseUrl + "messages",
      { observe: "response", params }).subscribe({
        next: response => setPaginationResponse(response, this.paginatedResult)
      });
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + "messages/thread/" + username);
  }
}
