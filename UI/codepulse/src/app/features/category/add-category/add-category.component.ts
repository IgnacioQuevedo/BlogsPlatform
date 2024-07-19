import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models (Objects)/add-category-request.model';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';


@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnDestroy
{

  categoryExample: AddCategoryRequest;
  private addCategorySubscription? : Subscription;


  constructor(private categoryService: CategoryService, private router: Router)
  {

  this.categoryExample = 
  {
    name : '',
    urlHandle: ''

  };

  }
onFormSubmit()
{
  this.addCategorySubscription = this.categoryService.addCategory(this.categoryExample)
  .subscribe({
    next: (Response) => {
      this.router.navigateByUrl('/admin/categories');

    },

  })

}

ngOnDestroy(): void {

  this.addCategorySubscription?.unsubscribe();
}



}