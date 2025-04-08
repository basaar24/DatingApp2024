import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {
        path: "",
        runGuardsAndResolvers: "always",
        canActivate: [authGuard],
        children:[
            {path: "members", component: MemberListComponent,
                canActivate: [authGuard]},
            {path: "members/:username", component: MemberDetailComponent,
                resolve: { member: memberDetailedResolver }},
            {path: "member/edit", component: MemberEditComponent,
                canDeactivate: [preventUnsavedChangesGuard]},
            {path: "lists", component: ListsComponent},
            {path: "messages", component: MessagesComponent},
        ]
    },
    {path: "errors", component: TestErrorsComponent},
    {path: "not-found", component: NotFoundComponent},
    {path: "server-error", component: ServerErrorComponent},
    {path: "**", component: HomeComponent, pathMatch: "full"},
];
