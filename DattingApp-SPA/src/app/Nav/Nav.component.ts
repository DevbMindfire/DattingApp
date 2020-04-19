import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {

  model: any =  {};
  constructor(public authService: AuthService, private alertifyService: AlertifyService ,private router: Router) { }

  ngOnInit() {
  }

  Login(){

    this.authService.Login(this.model).subscribe(next =>{

      this.alertifyService.Sucess('Succesfully login');

    }, error => {

      this.alertifyService.Error(error);

    }, () => {

      this.router.navigate(['/Member']);

    });

  }
  LoggedIn(){
    return this.authService.LoggedIn();
  }
  LoggedOut(){
    localStorage.removeItem('Token');
    this.alertifyService.Warning('Sucesfully log out');
    this.router.navigate(['/Home']);
  }

}
