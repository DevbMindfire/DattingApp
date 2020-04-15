import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  model: any = { };
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  Register(){
    this.authService.Register(this.model).subscribe(() => {
      console.log('Succesfully register');
    }, error => {
      console.log(error);
    });
  }
  Cancel(){
    this.cancelRegister.emit(false);
    console.log('Sucessfully canceled');
  }

}
