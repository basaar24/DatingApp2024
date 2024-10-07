import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {path: "members", component: MemberListComponent},
    {path: "members/:id", component: MemberDetailComponent},
    {path: "lists", component: ListsComponent},
    {path: "messages", component: MessagesComponent},
    {path: "**", component: HomeComponent, pathMatch: "full"},
];
