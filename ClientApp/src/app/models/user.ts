import { Image } from "./image";

export class User {
  id:number
  username:string
  name:string
  age:number
  gender:string
  created:Date
  lastActive:Date
  country:string
  city:string
  introduction:string
  hobbies:string
  profileImageUrl:string
  imageUrl:string
  image:Image
  images:Image[]
}
