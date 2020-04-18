import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';
import { AlertifyService } from '../_Service/Alertify.service';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  model: any = { };
  constructor(private authService: AuthService,private alertifyService: AlertifyService) { }

  ngOnInit() {
  }

  Register(){
    this.authService.Register(this.model).subscribe(() => {
      this.alertifyService.Sucess('Succesfully register');
    }, error => {
      this.alertifyService.Error(error);
      console.log(error);
    });
  }
  Cancel(){
    this.cancelRegister.emit(false);
    this.alertifyService.Message('Sucessfully canceled');
  }

}
