import { Component, OnInit, Input } from '@angular/core';
import { UserService } from 'src/app/_Service/User.service';
import { AlertifyService } from 'src/app/_Service/Alertify.service';
import { AuthService } from 'src/app/_Service/Auth.service';
import { Message } from 'src/app/_Model/Message';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-Member-Message',
  templateUrl: './Member-Message.component.html',
  styleUrls: ['./Member-Message.component.css']
})
export class MemberMessageComponent implements OnInit {

  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};
  constructor(private userService: UserService, private alertify: AlertifyService,
              private authservice: AuthService) { }

  ngOnInit() {
    this.LoadMessages();
  }

  LoadMessages(){
    const currentuserId = +this.authservice.decodedToken.nameid;
    this.userService.GetMessageThread(this.authservice.decodedToken.nameid, this.recipientId)
    .pipe(
      tap(messages => {
        for (let i = 0; i < messages.length; i++){
          if(messages[i].isRead === false && messages[i].recipientId === currentuserId){
            this.userService.MarkMessageAsRead(currentuserId, messages[i].id);
          }
        }
      })
    )
    .subscribe(use => {
      this.messages = use;
    } , error => {
      console.log(error);
      this.alertify.Error(error);
    });
  }

  SendMessage(){
    this.newMessage.recipientId = this.recipientId;
    this.userService.SendMessage(this.authservice.decodedToken.nameid, this.newMessage).subscribe((message: Message) => {
      this.messages.unshift(message);
      this.newMessage.content = '';
    } , error => {
      console.log(error);
      this.alertify.Error(error);
    });
  }
}
