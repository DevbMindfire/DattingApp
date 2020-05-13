import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_Model/User';
import { UserService } from '../_Service/User.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Message } from '../_Model/Message';
import { AuthService } from '../_Service/Auth.service';

@Injectable()

export class MessageResolver implements Resolve<Message[]>{
     constructor(private userService: UserService, private router: Router,
                 private alertifyService: AlertifyService, private authService: AuthService){}

     pageNumber = 1;
     pageSize = 5;
     messageContainer = 'Unread';

     resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
          return this.userService.GetMessages(this.authService.decodedToken.nameid, this.pageNumber,
                                              this.pageSize, this.messageContainer).pipe(
               catchError(error => {
                    this.alertifyService.Error('Problem retreving Messages');
                    this.router.navigate(['/Home']);
                    return of(null);
               })
          );
     }
}