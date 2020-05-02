import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_Service/Auth.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_Model/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Register',
  templateUrl: './Register.component.html',
  styleUrls: ['./Register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  user: User;
  RegisterForm: FormGroup;
  bsDatePickerConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private router: Router,
              private alertifyService: AlertifyService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.bsDatePickerConfig = {
      containerClass: 'theme-red'
    },
    this.CreateRegisterForm();
  }

   CreateRegisterForm(){
    this.RegisterForm = this.formBuilder.group({
      Gender: ['Male'],
      UserName: ['', Validators.required],
      KnownAs: ['', Validators.required],
      DateOfBirth: [null, Validators.required],
      City: ['', Validators.required],
      Country: ['', Validators.required],
      Password: ['', [Validators.required,
          Validators.minLength(4), Validators.maxLength(8)]],
      ConfirmPassword: ['', [Validators.required]]
    }, {validators: this.PasswordMatchValidator}
    );
  }
  PasswordMatchValidator(fromGroup: FormGroup){
    return fromGroup.get('Password').value === fromGroup.get('ConfirmPassword').value ? null : { 'mismatch': true};
  }

  Register(){

    if (this.RegisterForm.valid){
      this.user = Object.assign({}, this.RegisterForm.value);
      this.authService.Register(this.user).subscribe(() => {
        this.alertifyService.Sucess('Succesfully register');
      }, error => {
        this.alertifyService.Error(error);
        this.alertifyService.Error(error);
      }, () => {
        this.authService.Login(this.user).subscribe(() => {
          this.router.navigate(['/Members'])
        });
      });
    }
  }
  Cancel(){
    this.cancelRegister.emit(false);
    this.alertifyService.Message('Sucessfully canceled');
  }

}
