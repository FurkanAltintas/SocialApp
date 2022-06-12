export class Product {
  id:number;
  name:string;
  price:number;
  isActive:boolean;

  constructor(id:number, name:string, price:number, isActive:boolean) {
    this.id = id;
    this.name = name;
    this.price = price;
    this.isActive = isActive;
  }
}
