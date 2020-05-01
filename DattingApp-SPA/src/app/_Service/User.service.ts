import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_Model/User';


@Injectable({
  providedIn: 'root'
})
export class UserService {

baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

GetUsers(): Observable<User[]>{
  return this.http.get<User[]>(this.baseUrl + 'User');
}

GetUser(id): Observable<User>{
  return this.http.get<User>(this.baseUrl + 'User/' + id);
}

UpdateUser(id: number, user: User): Observable<object>{
  return this.http.put(this.baseUrl + 'User/' + id, user);
}

SetMainPhoto(userId: number, id: number){
  return this.http.post(this.baseUrl + 'User/' + userId + '/photos/' + id +'/setMain', {});
}

DeletePhoto(userId: number, id: number){
  return this.http.delete(this.baseUrl + 'User/' + userId + '/photos/' + id );
}
}
