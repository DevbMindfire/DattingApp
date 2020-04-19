import { Injectable } from '@angular/core';
import { CanActivate, Router} from '@angular/router';
import { AuthService } from '../_Service/Auth.service';
import { AlertifyService } from '../_Service/Alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router, private alertify: AlertifyService){}

  canActivate(): boolean {
    if (this.authService.LoggedIn()){
      return  true;
    }

    this.alertify.Warning('this is not permited ');
    this.router.navigate(['/Home']);
  } 
}
