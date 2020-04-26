import { Component, OnInit } from '@angular/core';
import { User } from '../../_Model/User';
import { UserService } from '../../_Service/User.service';
import { AlertifyService } from '../../_Service/Alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-Member-List',
  templateUrl: './Member-List.component.html',
  styleUrls: ['./Member-List.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];
  constructor(private userService: UserService, private alertifyService: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'];
    });
  }

  // LoadUser(){
  //   this.userService.GetUsers().subscribe((users: User[]) => {
  //     this.users = users;
  //   }, error => {
  //     this.alertifyService.Error(error);
  //   });
  // }

}
