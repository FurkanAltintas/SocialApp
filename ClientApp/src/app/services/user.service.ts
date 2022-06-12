import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "../models/user";

// const httpOptions = {
//   headers: new HttpHeaders({
//     'Authorization': `Bearer ${localStorage.getItem('token')}`
//   })
// }

@Injectable({
  providedIn: 'root'
})

export class UserService {

  baseUrl:string = "https://localhost:7029/api/users"

  constructor(private http:HttpClient) {}

  getUsers(userParams?):Observable<User[]> {

    let params = new HttpParams()

    if (userParams) {
      if (userParams.gender) {
        params = params.append('gender', userParams.gender)
      } if (userParams.minAge) {
        params = params.append('minAge', userParams.minAge)
      } if (userParams.maxAge) {
        params = params.append('maxAge', userParams.maxAge)
      } if (userParams.country) {
        params = params.append('country', userParams.country)
      } if (userParams.city) {
        params = params.append('city', userParams.city)
      } if (userParams.orderby) {
        params = params.append('orderby', userParams.orderby)
      }

      // append => Belirtilen bir nesne üzerininin sonuna html tagı veya metin eklemeye yarar.
    }

    return this.http.get<User[]>(this.baseUrl, { params: params });
  }

  follows(followParams):Observable<User[]> {
    let params = new HttpParams();
    if (followParams == 'followers') {
      params = params.append('followers', 'true')
    } if (followParams == 'followings') {
      params = params.append('followings', 'true')
    }

    return this.http.get<User[]>(`${this.baseUrl}/follows`, { params: params });
  }

  getUser(id:number):Observable<User> {
    return this.http.get<User>(`${this.baseUrl}/${id}`)
  }

  updateUser(id: number, user: User) {
    return this.http.put(`${this.baseUrl}/${id}`, user)
  }

  followUser(followerId: number, userId: number) {
    return this.http.post(`${this.baseUrl}/${followerId}/follow/${userId}`, {})
  }

  isFollow(userId:number):Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/isFollow/${userId}`)
  }

}
