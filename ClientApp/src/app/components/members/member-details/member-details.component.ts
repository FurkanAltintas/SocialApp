import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { MessageCreateComponent } from '../../messages/message-create/message-create.component';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
})
export class MemberDetailsComponent implements OnInit {

  user: User;
  followText:string = 'Takip Et'
  follow:boolean = false

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.user = data['user'];
    });

    this.isFollow(this.user.id)
  }

  followUser(userId: number) {
    this.userService.followUser(this.authService.decodedToken.nameid, userId).subscribe(result => {
      this.alertifyService.success(`${this.user.name} kullanıcısını takip ediyorsunuz`)
      this.follow = true
    }, error => {
      this.alertifyService.error(error)
    })
  }

  isFollow(userId:number):void {
    this.userService.isFollow(userId).subscribe(data => {
      this.follow = data
      if (this.follow) {
        this.followText = 'Takip Ediliyor'
      }
      console.log(this.follow)
    });
  }

  sendMessage() {
    const modalRef = this.modalService.open(MessageCreateComponent)
    modalRef.componentInstance.recipientId = this.user.id
  }
}
