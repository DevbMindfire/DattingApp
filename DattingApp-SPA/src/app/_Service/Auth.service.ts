import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.apiUrl + 'Auth/';
  private jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient) {}

  Login(model: any){
    return this.http.post(this.baseUrl + 'Login', model).pipe(
      map((response: any) => {
        const User = response;
        if (User){
          localStorage.setItem('Token' , User.token);
          this.decodedToken = this.jwtHelper.decodeToken(User.token);
        }
      })
    );
  }

  Register(model: any){
    return this.http.post(this.baseUrl + 'Register', model);
  }

  LoggedIn(){
    const token = localStorage.getItem('Token');
    return !this.jwtHelper.isTokenExpired(token);
  }

}
