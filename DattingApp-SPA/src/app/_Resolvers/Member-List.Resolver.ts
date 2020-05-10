import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_Model/User';
import { UserService } from '../_Service/User.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()

export class MemeberListResolver implements Resolve<User[]>{
     constructor(private userService: UserService, private router: Router, private alertifyService: AlertifyService){}

     pageNumber = 1;
     pageSize = 5;

     resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
          return this.userService.GetUsers(this.pageNumber, this.pageSize).pipe(
               catchError(error => {
                    this.alertifyService.Error('Problem retreving Data');
                    this.router.navigate(['/Home']);
                    return of(null);
               })
          );
     }
}