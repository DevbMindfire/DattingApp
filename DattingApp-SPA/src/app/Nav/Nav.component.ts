import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';

@Component({
  selector: 'app-Nav',
  templateUrl: './Nav.component.html',
  styleUrls: ['./Nav.component.css']
})
export class NavComponent implements OnInit {

  model: any =  {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  Login(){

    this.authService.Login(this.model).subscribe(next =>{

      console.log('Succesfully login');

    }, error => {

      console.log('Failed To login');

    });

  }
  LoggedIn(){

   const token = localStorage.getItem('Token');

   return !!token;


  }
  LoggedOut(){
    localStorage.removeItem('Token');
    console.log('Sucesfully log out');
  }

}
