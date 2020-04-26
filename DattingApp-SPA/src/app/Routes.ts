import { Routes } from '@angular/router';
import { HomeComponent } from './Home/Home.component';
import { ListComponent } from './List/List.component';
import { MemberListComponent } from './Member/Member-List/Member-List.component';
import { MessageComponent } from './Message/Message.component';
import { AuthGuard } from './_Guard/auth.guard';
import { MemberDetailComponent } from './Member/Member-Detail/Member-Detail.component';
import { MemeberDetailResolver } from './_Resolvers/Member-Detail.Resolver';
import { MemeberListResolver } from './_Resolvers/Member-List.Resolver';
import { MemberEditComponent } from './Member/Member-Edit/Member-Edit.component';
import { MemeberEditResolver } from './_Resolvers/Member-Edit.Resolver';
import { PreventUnsavedChanges } from './_Guard/prevent-unsaved-changes.gaurd';

export const AppRoutes: Routes = [
     {path: '' , component: HomeComponent},
     // this section helps us to use Protection in every url
     {
          path: '',
          runGuardsAndResolvers: 'always',
          canActivate: [AuthGuard],
          children: [
               {path: 'Members', component: MemberListComponent,
                     resolve: {users: MemeberListResolver}},
               {path: 'Members/:id' , component: MemberDetailComponent,
                     resolve: {user: MemeberDetailResolver}},
               {path: 'Member/Edit' , component: MemberEditComponent,
                     resolve: {user: MemeberEditResolver},
                        canDeactivate: [PreventUnsavedChanges]},
               {path: 'Message' , component: MessageComponent},
               {path: 'List' , component: ListComponent}

          ]
     },
     {path: '**' , redirectTo: '', pathMatch: 'full'},
];
