import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/product';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  selectedProduct: Product;
  products:Product[]=[];

  constructor(private productService:ProductService) { }

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts() {
    this.productService.getProducts().subscribe(products => {
      this.products = products;
    });
  }

  onSelectProduct(product:Product) {
    this.selectedProduct = product;
  }

  deleteProduct(product:Product):void {
    this.productService.deleteProduct(product).subscribe(data => {
      this.products.splice(this.products.findIndex(p => p.id == product.id), 1)
    });
  }

}
