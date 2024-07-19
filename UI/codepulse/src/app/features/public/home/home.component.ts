import { Component, OnInit } from '@angular/core';
import { BlogpostService } from '../../blog-posts/services/blogpost.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-posts/Models (Objects)/blogpost.model';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit
{

  BlogPosts$? : Observable<BlogPost[]>

  constructor(private blogPostService : BlogpostService) 
  {
    
  }
  ngOnInit(): void
  {
    this.BlogPosts$ = this.blogPostService.getAllBlogPosts();
  }

}
