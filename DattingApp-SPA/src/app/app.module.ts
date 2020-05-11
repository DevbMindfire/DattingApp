import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { NavComponent } from './Nav/Nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule, TabsModule, PaginationModule } from 'ngx-bootstrap';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule  } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TimeagoModule } from 'ngx-timeago';

import { AppComponent } from './app.component';
import { AuthService } from './_Service/Auth.service';
import { HomeComponent } from './Home/Home.component';
import { RegisterComponent } from './Register/Register.component';
import { ErrorInterCeptorProvide } from './_Service/Error.interceptor';
import { MemberListComponent } from './Member/Member-List/Member-List.component';
import { ListComponent } from './List/List.component';
import { MessageComponent } from './Message/Message.component';
import { AppRoutes } from './Routes';
import { MemberCardComponent } from './Member/Member-Card/Member-Card.component';
import { UserService } from './_Service/User.service';
import { AlertifyService } from './_Service/Alertify.service';
import { MemberDetailComponent } from './Member/Member-Detail/Member-Detail.component';
import { MemeberDetailResolver } from './_Resolvers/Member-Detail.Resolver';
import { AuthGuard } from './_Guard/auth.guard';
import { MemeberListResolver } from './_Resolvers/Member-List.Resolver';
import { MemberEditComponent } from './Member/Member-Edit/Member-Edit.component';
import { MemeberEditResolver } from './_Resolvers/Member-Edit.Resolver';
import { PreventUnsavedChanges } from './_Guard/prevent-unsaved-changes.gaurd';
import { PhotoEditorComponent } from './Member/Photo-Editor/Photo-Editor.component';
import { from } from 'rxjs';
import { ListResolver } from './_Resolvers/List.Resolver';



export function TokenGetter(){
   return localStorage.getItem('Token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListComponent,
      MessageComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      PhotoEditorComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      BrowserAnimationsModule,
      BsDatepickerModule.forRoot(),
      RouterModule.forRoot(AppRoutes),
      NgxGalleryModule,
      FileUploadModule,
      TabsModule.forRoot(),
      JwtModule.forRoot({
         config: {
            tokenGetter: TokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
      TimeagoModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot()
   ],
   providers: [
      AuthService,
      ErrorInterCeptorProvide,
      AlertifyService,
      AuthGuard,
      UserService,
      MemeberDetailResolver,
      MemeberListResolver,
      MemeberEditResolver,
      PreventUnsavedChanges,
      ListResolver
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
