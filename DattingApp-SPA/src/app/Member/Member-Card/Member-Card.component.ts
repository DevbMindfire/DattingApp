import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_Model/User';
import { UserService } from 'src/app/_Service/User.service';
import { AlertifyService } from 'src/app/_Service/Alertify.service';
import { AuthService } from 'src/app/_Service/Auth.service';

@Component({
  selector: 'app-Member-Card',
  templateUrl: './Member-Card.component.html',
  styleUrls: ['./Member-Card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;
  constructor(private userSerive: UserService, private alertify: AlertifyService,
              private authServie: AuthService) { }

  ngOnInit() {
  }

  SendLike(id: number){
    this.userSerive.SendLike(this.authServie.decodedToken.nameid,id).subscribe(use => {
      this.alertify.Sucess('You have liked ' + this.user.knowsAs);
    }, error => {
      this.alertify.Error(error);
    });
  }


}
