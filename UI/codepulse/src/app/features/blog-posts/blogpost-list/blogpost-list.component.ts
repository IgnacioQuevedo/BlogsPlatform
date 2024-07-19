import { Component, OnInit } from '@angular/core';
import { BlogpostService } from '../services/blogpost.service';
import { Observable, Subscription } from 'rxjs';
import { BlogPost } from '../Models (Objects)/blogpost.model';

@Component({
  selector: 'app-blogpost-list',
  templateUrl: './blogpost-list.component.html',
  styleUrls: ['./blogpost-list.component.css']
})
export class BlogpostListComponent implements OnInit
{

  blogPosts$? : Observable<BlogPost[]>;


constructor(private blogPostService : BlogpostService)
{

}
 
ngOnInit() : void
{

  this.blogPosts$ = this.blogPostService.getAllBlogPosts();
}




}


