import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddBlogPosts } from '../Models (Objects)/add-blogpost.model';
import { BlogPost } from '../Models (Objects)/blogpost.model';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UpdateBlogPost } from '../Models (Objects)/update-blogpost-model';

@Injectable({
  providedIn: 'root'
})
export class BlogpostService {
  
  constructor(private http : HttpClient) { }

  //LA RAZON DEL POR QUÉ SE DEVUELVE UN OBSERVABLE SE DEBE A QUE 

  createBlogPost(data: AddBlogPosts) : Observable<BlogPost>
  {
    return this.http.post<BlogPost>(`${environment.apiBaseUrl}/api/blogposts?addAuth=true`,data);
  }

  getAllBlogPosts() : Observable<BlogPost[]> {
    return this.http.get<BlogPost[]>(`${environment.apiBaseUrl}/api/Blogposts`);
  }

  getBlogPostById(id : string) : Observable<BlogPost>{

    return this.http.get<BlogPost>(`${environment.apiBaseUrl}/api/Blogposts/${id}`);
  }

  getBlogPostByUrlHandle(urlHandle: string) : Observable<BlogPost>
  {
    
    return this.http.get<BlogPost>(`${environment.apiBaseUrl}/api/Blogposts/${urlHandle}`);

  }

  updateBlogPost(idToUpd: string, updates : UpdateBlogPost) : Observable<BlogPost> {

    return this.http.put<BlogPost>(`${environment.apiBaseUrl}/api/Blogposts/${idToUpd}?addAuth=true`,updates);
  }
  deleteBlogPostById(idToDelete: string) : Observable<BlogPost>{
    
    return this.http.delete<BlogPost>(`${environment.apiBaseUrl}/api/Blogposts/${idToDelete}?addAuth=true`);
  }

  
}
