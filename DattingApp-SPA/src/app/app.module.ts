import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BsDropdownModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './Nav/Nav.component';
import {FormsModule} from '@angular/forms';
import { AuthService } from './_Service/Auth.service';
import { HomeComponent } from './Home/Home.component';
import { RegisterComponent } from './Register/Register.component';
import { ErrorInterCeptorProvide } from './_Service/Error.interceptor';
import { MemberListComponent } from './Member-List/Member-List.component';
import { ListComponent } from './List/List.component';
import { MessageComponent } from './Message/Message.component';
import { AppRoutes } from './Routes';



@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListComponent,
      MessageComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      RouterModule.forRoot(AppRoutes)
   ],
   providers: [
      AuthService,
      ErrorInterCeptorProvide
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
