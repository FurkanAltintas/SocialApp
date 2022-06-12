import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Login } from '../models/login';
import { Register } from '../models/register';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl: string = 'https://localhost:7029/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken:any;

  constructor(private http: HttpClient) {}

  login(user: Login) {
    return this.http.post(`${this.baseUrl}login`, user).pipe(
      map((response: any) => {
        const result = response;
        if (result) {
          localStorage.setItem('token', result.token);
          // Application -> Local Storage -> http://localhost:4200
          this.decodedToken = this.jwtHelper.decodeToken(result.token)
        }
      })
    );
  }

  register(user: Register) {
    return this.http.post(`${this.baseUrl}register`, user);
  }

  loggedIn() {
    const token = localStorage.getItem('token')
    return !this.jwtHelper.isTokenExpired(token); // Süresi bitmiş ise
  }

  logout() {
    localStorage.removeItem('token')
  }
}
