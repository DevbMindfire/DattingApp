import { Component, OnInit } from '@angular/core';
import { UserService } from '../_Service/User.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_Service/Auth.service';
import { User } from '../_Model/User';
import { Pagination, PaginatedResult } from '../_Model/Pagination';

@Component({
  selector: 'app-List',
  templateUrl: './List.component.html',
  styleUrls: ['./List.component.css']
})
export class ListComponent implements OnInit {

  users: User[];
  pagination: Pagination;
  likeParams: string;
  constructor(private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute, private authSerice: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
    this.likeParams = 'likers';
  }
  LoadUser(){
    this.userService.GetUsers(this.pagination.currentPage , this.pagination.itemsPerPage, null, this.likeParams).
    subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.Error(error);
    });
  }
  pageChanged(event: any){
    this.pagination.currentPage = event.page;
    this.LoadUser();
  }
}
