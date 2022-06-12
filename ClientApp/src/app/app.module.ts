import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TimeagoModule } from 'ngx-timeago';
import { ngxLoadingAnimationTypes, NgxLoadingModule } from 'ngx-loading';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'


export function tokenGetter() {
  return localStorage.getItem('token');
}


import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ProductsComponent } from './components/products/products.component';
import { ProductFormComponent } from './components/products/product-form/product-form.component';
import { ProductDetailsComponent } from './components/products/product-details/product-details.component';
import { RegisterComponent } from './components/register/register.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { HomeComponent } from './components/home/home.component';
import { NotfoundComponent } from './components/notfound/notfound.component';
import { MemberDetailsComponent } from './components/members/member-details/member-details.component';
import { PhotoGalleryComponent } from './components/photo-gallery/photo-gallery.component';
import { MemberEditComponent } from './components/members/member-edit/member-edit.component';
import { MessageCreateComponent } from './components/messages/message-create/message-create.component';
import { MessageListComponent } from './components/messages/message-list/message-list.component';


import { appRoutes } from './routes';


import { AuthGuard } from './guards/auth-guard';
import { ErrorInterceptor } from './services/error.interceptor';
import { MemberEditResolver } from './resolvers/member-edit.resolver';
import { MemberDetailsResolver } from './resolvers/member-details.resolver';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ProductsComponent,
    ProductFormComponent,
    ProductDetailsComponent,
    RegisterComponent,
    MemberListComponent,
    FriendListComponent,
    HomeComponent,
    NotfoundComponent,
    MemberDetailsComponent,
    PhotoGalleryComponent,
    MemberEditComponent,
    MessageCreateComponent,
    MessageListComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule, // ngModel'i formlarda kullanabilmek için ilgili componentin dahil olduğu module içerisine FormsModule eklenmesi gerekiyor.
    NgbModule,
    TimeagoModule.forRoot(),
    NgxLoadingModule.forRoot({
      animationType: ngxLoadingAnimationTypes.pulse,
      backdropBackgroundColour: 'rgba(0,0,0,0.1)',
      backdropBorderRadius: '4px',
      primaryColour: '#ffffff',
      secondaryColour: '#ffffff',
      tertiaryColour: '#ffffff'
    }),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ['localhost:7029'],
        disallowedRoutes: ['localhost:7029/api/auth'],
      },
    }),
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorInterceptor,
      multi: true,
    },
    MemberEditResolver,
    MemberDetailsResolver
  ],
  bootstrap: [AppComponent], // Başlangıç Componenti
})
export class AppModule {}
