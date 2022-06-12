import { Routes } from "@angular/router";
import { FriendListComponent } from "./components/friend-list/friend-list.component"
import { HomeComponent } from "./components/home/home.component"
import { MemberDetailsComponent } from "./components/members/member-details/member-details.component"
import { MemberEditComponent } from "./components/members/member-edit/member-edit.component"
import { MemberListComponent } from "./components/members/member-list/member-list.component"
import { MessageListComponent } from "./components/messages/message-list/message-list.component"
import { NotfoundComponent } from "./components/notfound/notfound.component"
import { RegisterComponent } from "./components/register/register.component"
import { AuthGuard } from "./guards/auth-guard"
import { MemberDetailsResolver } from "./resolvers/member-details.resolver"
import { MemberEditResolver } from "./resolvers/member-edit.resolver"

export const appRoutes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo:'home', pathMatch:'full' }, // Herhangi bir link girilmediği zaman HomeComponent çalışacak
  { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] },
  { path: 'member/edit', component: MemberEditComponent, canActivate: [AuthGuard], resolve: { user: MemberEditResolver } },
  { path: 'members/:id', component: MemberDetailsComponent, canActivate: [AuthGuard], resolve: { user: MemberDetailsResolver } },
  { path: 'friends', component: FriendListComponent, canActivate: [AuthGuard] },
  { path: 'messages', component: MessageListComponent},
  { path: 'register', component: RegisterComponent},
  { path: '**', component: NotfoundComponent} // Buradaki linklerin hiç biri karşılanmıyorsa
]
