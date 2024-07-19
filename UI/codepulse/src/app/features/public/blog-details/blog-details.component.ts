import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogpostService } from '../../blog-posts/services/blogpost.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-posts/Models (Objects)/blogpost.model';

@Component({
  selector: 'app-blog-details',
  templateUrl: './blog-details.component.html',
  styleUrls: ['./blog-details.component.css']
})
export class BlogDetailsComponent implements OnInit
{
  urlHandle? : string | null = null;
  blogPostToShow$? : Observable<BlogPost>
  blogPost? : BlogPost

  constructor(private blogPostService : BlogpostService, private route : ActivatedRoute) 
  {
    
  }
  ngOnInit(): void 
  {

    this.route.paramMap
    .subscribe({
      next: (params) => {
        this.urlHandle = params.get('url');
      }
    })

    //Fetch blog details by url
    if(this.urlHandle)
    {
      this.blogPostToShow$ = this.blogPostService.getBlogPostByUrlHandle(this.urlHandle);
    }
  }
}
