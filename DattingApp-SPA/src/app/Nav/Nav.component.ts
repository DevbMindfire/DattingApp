import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';
import { AlertifyService } from '../_Service/Alertify.service';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {

  model: any =  {};
  constructor(public authService: AuthService,private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  Login(){

    this.authService.Login(this.model).subscribe(next =>{

      this.alertifyService.Sucess('Succesfully login');

    }, error => {

      this.alertifyService.Error(error);

    });

  }
  LoggedIn(){
    return this.authService.LoggedIn();
  }
  LoggedOut(){
    localStorage.removeItem('Token');
    this.alertifyService.Warning('Sucesfully log out');
  }

}
