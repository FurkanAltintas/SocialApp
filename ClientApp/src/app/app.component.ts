import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {

  title = 'SocialApp';
  jwtHelper = new JwtHelperService()

  constructor(private authSerivce:AuthService) {}

  ngOnInit(): void { // Herhangi bir component yüklenmeden önce ngOnInit metodu çalışacak
      const token = localStorage.getItem('token')
      if (token) {
        this.authSerivce.decodedToken = this.jwtHelper.decodeToken('token')
      }
  }
}

