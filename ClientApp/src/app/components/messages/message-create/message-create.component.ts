import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-message-create',
  templateUrl: './message-create.component.html',
  styleUrls: ['./message-create.component.css'],
})
export class MessageCreateComponent implements OnInit {
  @Input() recipientId: number;
  message: any = {};

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private authService: AuthService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit(): void {
    console.log(this.recipientId);
  }

  sendMessage() {
    this.message.recipientId = this.recipientId

    this.messageService.sendMessage(this.authService.decodedToken.nameid, this.message)
      .subscribe(result => {
      console.log(result)
      this.alertifyService.success('Mesaj Gönderildi')
      this.close()
    }, error => {
      this.alertifyService.error(error)
    });
  }

  close(): void {
    this.activeModal.close();
  }
}
