import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_Model/User';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.apiUrl + 'Auth/';
  private jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/aki.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) {}

  ChangeMemberPhoto(photoUrl: string){
    this.photoUrl.next(photoUrl);
  }

  Login(model: any){
    return this.http.post(this.baseUrl + 'Login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user){
          localStorage.setItem('Token' , user.token);
          localStorage.setItem('User',JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
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
