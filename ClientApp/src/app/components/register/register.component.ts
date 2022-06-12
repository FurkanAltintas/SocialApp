import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { Register } from '../../models/register';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  user: Register = {
    name: null,
    email: null,
    username: null,
    password: null,
    gender: null,
    dateOfBirth: new Date(),
    country:null,
    city: null
  };

  constructor(private authService: AuthService, private alertifyService:AlertifyService, private route: Router) {}

  ngOnInit(): void {}

  register() {
    this.authService.register(this.user).subscribe(
      () => {
        this.alertifyService.success('kullanıcı oluşturuldu')
      },
      (error) => {
        this.alertifyService.error(error)
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.route.navigate(['/members'])
        })
      }
    );
  }
}
