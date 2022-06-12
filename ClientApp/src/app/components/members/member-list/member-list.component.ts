import { Component, OnInit } from '@angular/core';
import { User } from '../../../models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[]
  public loading: boolean = false
  userParams: any = {}

  constructor(private userService:UserService,private authService:AuthService, private alertifyService:AlertifyService) { }

  ngOnInit(): void {
    this.userParams.orderby = 3

    this.getUsers();
  }

  getUsers():void {
    this.loading = true

    console.log(this.userParams)

    this.userService.getUsers(this.userParams).subscribe(users => {
      this.loading = false
      this.users = users
    }, error => {
      this.loading = false
      this.alertifyService.error(error)
    })
  }
}
