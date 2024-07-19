import { Component, OnDestroy, OnInit } from '@angular/core';
import { BlogPost } from '../Models (Objects)/blogpost.model';
import { BlogpostService } from '../services/blogpost.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models (Objects)/category.model';
import { UpdateBlogPost } from '../Models (Objects)/update-blogpost-model';
import { ImageService } from 'src/app/shared/components/image-selector/image.service';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit, OnDestroy
{

  blogFound? : BlogPost;
  idOfBlog : string | null = null;

  routeSubscription? : Subscription;
  updateBlogPostSubscription? : Subscription;
  getBlogPostSubscription?: Subscription;
  deleteBlogPostSubscription?: Subscription;
  imageSelectSubscription? : Subscription;

  allCategories$? : Observable<Category[]>;
  selectedCategories? : string[];

  isImageSelectorVisible : boolean = false;

  constructor(
    private blogPostService : BlogpostService, 
    private categoryService : CategoryService,
    private route : ActivatedRoute, private router : Router,
    private imageService : ImageService
    )
  {
  }

  onFormSubmit() : void
  {

    if(this.blogFound && this.idOfBlog){

      var blogPostWithUpd: UpdateBlogPost = {

        title : this.blogFound.title,
        author : this.blogFound.author,
        shortDescription : this.blogFound.shortDescription,
        content : this.blogFound.content,
        featuredImageUrl : this.blogFound.featuredImageUrl,
        isVisible : this.blogFound.isVisible,
        publishedDate : this.blogFound.publishedDate,
        urlHandle : this.blogFound.urlHandle,
        categories : this.selectedCategories ?? []
      };

      //BlogPostWithUpd no lleva THIS. porque es una variable local.
      this.updateBlogPostSubscription = this.blogPostService.updateBlogPost(this.idOfBlog, blogPostWithUpd)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl("/admin/blogposts");
        }
      });
    }
  }

  deleteBlogPost() : void 
  {
    if(this.idOfBlog){
      this.deleteBlogPostSubscription = this.blogPostService.deleteBlogPostById(this.idOfBlog)
      .subscribe({
        next: (response) =>{
          this.selectedCategories = undefined;
          this.router.navigateByUrl("admin/blogposts");
        }
      });
  }

  }

  openImageSelector() : void
  {
    this.isImageSelectorVisible = true;
  }

  closeImageSelector() : void
  {
    this.isImageSelectorVisible = false;
  }

  
  ngOnInit(): void
  {

      this.allCategories$ = this.categoryService.getAllCategories();


      this.routeSubscription = this.route.paramMap.subscribe({
      next: (params) => {
        this.idOfBlog = params.get('id');
      }
    })
    if(this.idOfBlog){

      this.getBlogPostSubscription = this.blogPostService.getBlogPostById(this.idOfBlog)
      .subscribe({
        next: (response) => {
          this.blogFound = response;
          this.selectedCategories = response.categories.map(x => x.id)
        }
      });

    }

    this.imageService.onSelectImage()
    .subscribe({
      next: (response) => {
        if(this.blogFound){
          this.blogFound.featuredImageUrl = response.url;
          this.isImageSelectorVisible = false;
        }
      }
    })
  }


  ngOnDestroy(): void 
  {
    this.routeSubscription?.unsubscribe;
    this.updateBlogPostSubscription?.unsubscribe
    this.getBlogPostSubscription?.unsubscribe
    this.deleteBlogPostSubscription?.unsubscribe
    this.imageSelectSubscription?.unsubscribe
  }


}
