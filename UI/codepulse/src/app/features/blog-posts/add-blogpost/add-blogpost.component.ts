import { Component, OnDestroy, OnInit } from '@angular/core';
import { AddBlogPosts } from '../Models (Objects)/add-blogpost.model';
import { Observable, Subscription } from 'rxjs';
import { BlogpostService } from '../services/blogpost.service';
import { Router } from '@angular/router';
import { Category } from '../../category/models (Objects)/category.model';
import { CategoryService } from '../../category/services/category.service';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';

@Component({
  selector: 'app-add-blogpost',
  templateUrl: './add-blogpost.component.html',
  styleUrls: ['./add-blogpost.component.css']
})
export class AddBlogpostComponent implements OnDestroy, OnInit
{

  blogPostExample: AddBlogPosts;
  AddSubscription? : Subscription;
  allCategories$? : Observable<Category[]>;
  isImageSelectorVisible : boolean = false;
  imageSelectorSubscription? : Subscription;

  constructor(private router: Router, private blogPostService: BlogpostService, 
    private categoryService: CategoryService, private imageService : ImageService) 
  {

    this.blogPostExample =  {

    title: '',
    urlHandle: '',
    shortDescription: '',
    content: '',
    featuredImageUrl: '',
    publishedDate: new Date(),
    author: '',
    isVisible: true,
    categories: [],

    };

  }

  onFormSubmit() : void 
  {
   this.AddSubscription = this.blogPostService.createBlogPost(this.blogPostExample)
   .subscribe({
    next:(response) => {
      this.router.navigateByUrl('admin/blogposts');
    } 
   });

  }

  ngOnInit() : void 
  {

    this.allCategories$ = this.categoryService.getAllCategories();

    // En vez de esto, hago lo de arriba. Esto se debe a mayor eficacia y clean code,
    // ya que cuando quiero solamente mostrar valores del observable, es mejor usar el async pipe
    
    // this.categoryService.getAllCategories()
    // .subscribe({
    //   next: (response) => {
    //     this.allCategories = response;
    //   }
    // });


    this.imageSelectorSubscription = this.imageService.onSelectImage()
    .subscribe({
      next: (selectedImage) => {

        this.blogPostExample.featuredImageUrl = selectedImage.url;
        this.closeImageSelector();
      }
    });


  }

  openImageSelector() : void
  {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector() : void
  {
    this.isImageSelectorVisible = false;
  }

  ngOnDestroy() : void
  {
    this.AddSubscription?.unsubscribe;
    this.imageSelectorSubscription?.unsubscribe;
  }








}
