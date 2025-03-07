import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  membersCache = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));

  resetUserParams() {
    this.userParams.set(new UserParams(this.user));
  }

  getMembers() {
    let cacheKey = Object.values(this.userParams()).join("-");
    const cachedResponse = this.membersCache.get(cacheKey);

    if (cachedResponse) return this.setPaginationResponse(cachedResponse);

    let params = this.setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);

    params = params.append("minAge", this.userParams().minAge);
    params = params.append("maxAge", this.userParams().maxAge);
    params = params.append("gender", this.userParams().gender);
    params = params.append("orderBy", this.userParams().orderBy);

    return this.http.get<Member[]>(this.baseUrl + "users", { observe: "response", params }).subscribe({
      next: response => {
        this.setPaginationResponse(response);
        this.membersCache.set(cacheKey, response);
      }
    });
  }

  private setPaginationResponse(response: HttpResponse<Member[]>) {
    this.paginatedResult.set({
      items: response.body as Member[],
      pagination: JSON.parse(response.headers.get("Pagination")!)
    });
  }

  private setPaginationHeaders(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber && pageSize) {
      params = params.append("pageNumber", pageNumber);
      params = params.append("pageSize", pageSize);
    }

    return params;
  }

  getMember(username: string) {
    const member: Member = [...this.membersCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.userName === username);

    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + "users/" + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + "users", member).pipe(
      tap(() => {
        // this.members.update(members => 
        //   members.map(m => m.userName === member.userName ? member : m))
      })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + "users/photo/" + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photoUrl = photo.url;
      //     }
      //     return m;
      //   }))
      // })
    );
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + "users/photo/" + photo.id).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photos = m.photos.filter(p => p.id !== photo.id)
      //     }
      //     return m;
      //   }))
      // })
    );
  }
}
