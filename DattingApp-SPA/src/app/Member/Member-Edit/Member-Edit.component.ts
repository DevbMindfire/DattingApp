import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_Model/User';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_Service/Alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_Service/User.service';
import { AuthService } from 'src/app/_Service/Auth.service';

@Component({
  selector: 'app-Member-Edit',
  templateUrl: './Member-Edit.component.html',
  styleUrls: ['./Member-Edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('EditForm', {static: true}) editForm: NgForm;
  user: User;
  photoUrl: string;
  @HostListener('window:beforeunload', ['$event'])

  UnloadNotification($event: any){
    if (this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertifyService: AlertifyService,
              private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
     this.route.data.subscribe(data => {
      this.user = data['user'];
    });
     this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);
  }

  UpdateUser(){

    this.userService.UpdateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertifyService.Sucess('Portfolio Succesfully updated');
      this.editForm.reset(this.user);
    }, error => {
      this.alertifyService.Error(error);
    });
  }

  UpdateUserPhoto(url: string){
    this.user.photoUrls = url;  
  }

}
