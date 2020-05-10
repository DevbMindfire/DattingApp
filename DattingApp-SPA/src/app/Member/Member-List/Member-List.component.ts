import { Component, OnInit } from '@angular/core';
import { User } from '../../_Model/User';
import { UserService } from '../../_Service/User.service';
import { AlertifyService } from '../../_Service/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_Model/Pagination';

@Component({
  selector: 'app-Member-List',
  templateUrl: './Member-List.component.html',
  styleUrls: ['./Member-List.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];
  user: User = JSON.parse(localStorage.getItem('User'));
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];
  userParams: any = {};
  pagination: Pagination;
  constructor(private userService: UserService, private alertifyService: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
      console.log(this.pagination);
    });

    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
  }

  pageChanged(event: any){
    this.pagination.currentPage = event.page;
    this.LoadUser();
  }

  ResetFilters(){
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive';
    this.LoadUser();
  }

  SortOp(){
    this.userParams.orderBy = 'created';
    this.LoadUser();
  }

  SortLoad(){
    this.userParams.orderBy = 'lastActive';
    this.LoadUser();
  }

  LoadUser(){
    this.userService.GetUsers(this.pagination.currentPage , this.pagination.itemsPerPage , this.userParams).
    subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertifyService.Error(error);
    });
  }

}
