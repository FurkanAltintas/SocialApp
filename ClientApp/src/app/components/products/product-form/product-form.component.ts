import { Component, Input, OnInit } from '@angular/core';
import { Product } from '../../../models/product';
import { ProductService } from '../../../services/product.service';

@Component({
  selector: 'product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {

  @Input() products:Product[]

  constructor(private productService:ProductService) { }

  ngOnInit(): void {
  }

  addProduct(name:string, price:number, isActive:boolean) {
    const product = new Product(0, name, price, isActive);
    this.productService.addProduct(product).subscribe(product => {
      this.products.push(product)
    });
  }
}
