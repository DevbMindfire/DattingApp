import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_Service/Auth.service';
import { User } from './_Model/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  jwtHelper = new JwtHelperService();
  constructor(private authService: AuthService){}
  ngOnInit() {

    const Token = localStorage.getItem('Token');
    const user: User = JSON.parse(localStorage.getItem('User'));
    if (Token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(Token);
    }
    if (user){
      this.authService.currentUser = user;
      this.authService.ChangeMemberPhoto(user.photoUrls);
    }

  }
}
