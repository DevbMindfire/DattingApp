import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_Model/User';
import { UserService } from '../_Service/User.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_Service/Auth.service';

@Injectable()

export class MemeberEditResolver implements Resolve<User>{
     constructor(private userService: UserService, private router: Router, private authService: AuthService,
                 private alertifyService: AlertifyService){}

     resolve(route: ActivatedRouteSnapshot): Observable<User> {
          return this.userService.GetUser(this.authService.decodedToken.nameid).pipe(
               catchError(error => {
                    this.alertifyService.Error('Problem retreving Data');
                    this.router.navigate(['/Members']);
                    return of(null);
               })
          );
     }
}  