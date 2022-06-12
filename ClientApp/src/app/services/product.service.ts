import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
}) // Başka bir class içerisinde de kullanılabilir olacağını belirtiyor.
export class ProductService {

  baseUrl:string = "https://localhost:7029/api/products"

  constructor(private http:HttpClient) { }

  getProducts():Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl)
  }

  getProductById(id:number):Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`)
  }

  updateProduct(product:Product):Observable<Product> {
    return this.http.put<Product>(`${this.baseUrl}/${product.id}`, product)
  }

  addProduct(product:Product):Observable<Product> {
    return this.http.post<Product>(this.baseUrl, product)
  }

  deleteProduct(product:Product):Observable<Product> {
    return this.http.delete<Product>(`${this.baseUrl}/${product.id}`)
  }
}
