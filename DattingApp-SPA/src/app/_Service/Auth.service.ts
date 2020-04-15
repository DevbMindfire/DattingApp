import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:5000/api/Auth/';
  constructor(private http: HttpClient) {}

  Login(model: any){
    return this.http.post(this.baseUrl + 'Login', model).pipe(
      map((response: any) => {
        const User = response;
        if(User){
          localStorage.setItem('Token' , User.token);
        }
      })
    );
  }
  Register(model: any){
    return this.http.post(this.baseUrl + 'Register', model);
  }


}
