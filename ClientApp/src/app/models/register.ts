import { Login } from "./login";

export class Register extends Login {
  name:string;
  email:string;
  gender:string;
  dateOfBirth:Date;
  country:string;
  city:string;
}

// extends => kalıtım almak
