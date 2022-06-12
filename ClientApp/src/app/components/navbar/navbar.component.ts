import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { Login } from '../../models/login';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  user: Login = { username:null, password:null };
  name: string = 'USER_NAME'

  constructor(public authService:AuthService, private router:Router, private alertifyService:AlertifyService) { }

  ngOnInit(): void {
  }

  login() {
    this.authService.login(this.user).subscribe(next => {
      this.alertifyService.success('giriş başarılı')
      this.router.navigate(['/members']) // Kullanıcı login olduktan sonra members sayfasına yönlendiriyoruz
    }, error => {
      this.alertifyService.error(error)
    });
  }

  loggedIn():boolean {
    return this.authService.loggedIn();
  }

  logout() {
    this.authService.logout()
    this.alertifyService.warning('çıkış yapıldı')
    this.router.navigate(['/register'])
  }
}
