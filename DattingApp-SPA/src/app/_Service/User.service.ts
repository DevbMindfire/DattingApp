import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_Model/User';
import { PaginatedResult } from '../_Model/Pagination';
import { map, retry } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  GetUsers(page?, itemPerPage?, userParams?, likeParams?): Observable<PaginatedResult<User[]>>{
    const paginatedResult = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemPerPage != null){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemPerPage);
    }

    if (userParams != null) {
      params = params.append('minAge' , userParams.minAge);
      params = params.append('maxAge' , userParams.maxAge);
      params = params.append('gender' , userParams.gender);
      params = params.append('orderBy' , userParams.orderBy);
    }

    if (likeParams === 'Likers'){
      params = params.append('Likers', 'true');
    }

    if (likeParams === 'Likees'){
      params = params.append('Likees', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'User', {observe: 'response', params})
            .pipe(
              map(response => {
                paginatedResult.result = response.body;
                if (response.headers.get('Pagination') != null){
                  paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
                }
                return paginatedResult;
              })
            );
  }

  GetUser(id): Observable<User>{
    return this.http.get<User>(this.baseUrl + 'User/' + id);
  }

  UpdateUser(id: number, user: User): Observable<object>{
    return this.http.put(this.baseUrl + 'User/' + id, user);
  }

  SetMainPhoto(userId: number, id: number){
    return this.http.post(this.baseUrl + 'User/' + userId + '/photos/' + id + '/setMain', {});
  }

  DeletePhoto(userId: number, id: number){
    return this.http.delete(this.baseUrl + 'User/' + userId + '/photos/' + id );
  }

  SendLike(userId: number , recipientId: number){
    return this.http.post(this.baseUrl + 'User/' + userId + '/like/' + recipientId , {});
  }

}
