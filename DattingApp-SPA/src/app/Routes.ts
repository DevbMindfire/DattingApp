import { Routes } from '@angular/router';
import { HomeComponent } from './Home/Home.component';
import { ListComponent } from './List/List.component';
import { MemberListComponent } from './Member-List/Member-List.component';
import { MessageComponent } from './Message/Message.component';
import { AuthGuard } from './_Guard/auth.guard';

export const AppRoutes: Routes = [
     {path: '' , component: HomeComponent},
     //this section helps us to use Protection in every url 
     {
          path: '',
          runGuardsAndResolvers: 'always',
          canActivate: [AuthGuard],
          children: [
               {path: 'List' , component: ListComponent},
               {path: 'Member' , component: MemberListComponent},
               {path: 'Message' , component: MessageComponent}
          ]
     },
     {path: '**' , redirectTo: '', pathMatch: 'full'},
];