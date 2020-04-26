import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_Model/User';

@Component({
  selector: 'app-Member-Card',
  templateUrl: './Member-Card.component.html',
  styleUrls: ['./Member-Card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() user: User;
  constructor() { }

  ngOnInit() {
  }

}
