import { Component, OnInit } from '@angular/core';
import { Message } from '../_Model/Message';
import { Pagination, PaginatedResult } from '../_Model/Pagination';
import { UserService } from '../_Service/User.service';
import { AlertifyService } from '../_Service/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../_Service/Auth.service';

@Component({
  selector: 'app-Message',
  templateUrl: './Message.component.html',
  styleUrls: ['./Message.component.css']
})
export class MessageComponent implements OnInit {

  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Unread';

  constructor(private userService: UserService, private alertify: AlertifyService,
              private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.messages = data['messages'].result;
      this.pagination = data['messages'].pagination;
    });
  }

  LoadMessage(){
    this.userService.GetMessages(this.authService.decodedToken.nameid, this.pagination.currentPage,
          this.pagination.itemsPerPage , this.messageContainer).subscribe((res: PaginatedResult<Message[]>) => {
            this.messages = res.result;
            this.pagination = res.pagination;
    }, error =>{
      console.log(error);
      this.alertify.Error(error);
    });
  }

  PageChangeEvent(event: any){
    this.pagination.currentPage = event.page;
    this.LoadMessage();
  }

  DeleteMessage(id: number){
    this.alertify.Confirm('Are you sure you want to Delete this message', () => {
      this.userService.DeleteMessage(id, this.authService.decodedToken.nameid).subscribe(() => {
        this.messages.splice(this.messages.findIndex(u => u.id === id), 1);
        this.alertify.Sucess('Succesfully Deleted');
      }, error => {
        console.log(error);
        this.alertify.Error(error);
      });
    });
  }
}
