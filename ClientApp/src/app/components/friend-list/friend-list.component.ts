import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.css']
})
export class FriendListComponent implements OnInit {

  users:User[]
  followParams: string = 'followings'

  constructor(private userService:UserService,private authService:AuthService, private alertifyService:AlertifyService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers():void {
    this.userService.follows(this.followParams).subscribe(users => {
      this.users = users
    }, error => {
      this.alertifyService.error(error)
    })
  }

}
