import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Category } from '../models (Objects)/category.model';
import { CategoryService } from '../services/category.service';
import { UpdateCategoryRequest } from '../models (Objects)/update-category-request.model copy';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy
{

  id: string | null = null;
  paramsSubscription? : Subscription;
  editCategorySubscription? : Subscription;
  categoryFound? : Category

  constructor(private categoryService: CategoryService, 
    private route:ActivatedRoute,
    private router:Router)
  {



    
  }

  onFormSubmit() : void
  {
    const updateCategoryRequest: UpdateCategoryRequest = {
      name: this.categoryFound?.name ?? '',
      urlHandle: this.categoryFound?.urlHandle ?? ''
    };

    //pass this object to service
    if(this.id)
    {
      this.editCategorySubscription = this.categoryService.UpdateCategory(this.id,updateCategoryRequest)
      .subscribe({

        next:(response) => {
          
          this.router.navigateByUrl('admin/categories');
        }
      });
    }
    
  }


  onDelete() : void
  {
    if(this.id)
    {
      this.categoryService.deleteCategory(this.id)
      .subscribe({

        next: (response) => {
          this.router.navigateByUrl('admin/categories');
        }

      })
    }
    
  }




  ngOnInit(): void 
  {
    this.paramsSubscription = this.route.paramMap.subscribe({

      next: (params) => {
        this.id = params.get('id');

        if(this.id){

          //get the data from the API
          this.categoryService.getCategoryById(this.id)
          .subscribe({
            next: (response) =>{
              this.categoryFound = response
            } 
          });
        }
      }

    });

    
  }

  ngOnDestroy(): void 
  {
    this.paramsSubscription?.unsubscribe;
    this.editCategorySubscription?.unsubscribe;
  }

}
