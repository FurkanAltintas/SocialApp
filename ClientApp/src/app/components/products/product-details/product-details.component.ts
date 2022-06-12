import { Component, OnInit, Input } from '@angular/core';
import { Product } from '../../../models/product';
import { ProductService } from '../../../services/product.service';

@Component({
  selector: 'product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  @Input() product: Product;
  @Input() products: Product[];

  constructor(private productService:ProductService) { }

  ngOnInit(): void {
  }

  updateProduct(id:number, name:string, price:number, isActive:boolean):void {
    const product = new Product(id, name, price, isActive);
    this.productService.updateProduct(product).subscribe(result => {
      this.products.splice(this.products.findIndex(p => p.id==product.id), 1, product);
      // O index numarasından itibaren 1 tane kayıt silinecek ve güncellendikten sonra
      // yerine hangi objeyi eklemek istiyoruz onu belirtiyoruz
    });
    this.product = null;
  }

}
