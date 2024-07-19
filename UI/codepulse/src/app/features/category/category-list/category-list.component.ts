import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../services/category.service';
import { Category } from '../models (Objects)/category.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  
  categories$ ?: Observable<Category[]>



  constructor(private categoryService : CategoryService){


  }
 
  
  //Apenas cargue el componente quiero que la variable categories se inicialice
  ngOnInit(): void
  {
    this.categories$ = this.categoryService.getAllCategories();
  }
}
