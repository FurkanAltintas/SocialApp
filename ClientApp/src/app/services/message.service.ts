import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl:string = "https://localhost:7029/api/messages"

  constructor(private http:HttpClient) { }

  sendMessage(id:number, message:any) {
    return this.http.post(`${this.baseUrl}/${id}`, message)
  }

}
