import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryListComponent } from './features/category/category-list/category-list.component';
import { AddCategoryComponent } from './features/category/add-category/add-category.component';
import { EditCategoryComponent } from './features/category/edit-category/edit-category.component';
import { BlogpostListComponent } from './features/blog-posts/blogpost-list/blogpost-list.component';
import { AddBlogpostComponent } from './features/blog-posts/add-blogpost/add-blogpost.component';
import { EditBlogpostComponent } from './features/blog-posts/edit-blogpost/edit-blogpost.component';
import { HomeComponent } from './features/public/home/home.component';
import { BlogDetailsComponent } from './features/public/blog-details/blog-details.component';
import { LoginComponent } from './features/auth/login/login.component';
import { AuthGuard } from './features/auth/guards/auth.guard';
import { UnauthorizedComponent } from './core/unauthorized/unauthorized/unauthorized.component';

const routes: Routes = [

{
  path: '',
  component: HomeComponent
},

{
  path: "blog/:url",
  component: BlogDetailsComponent
},

{
  path: 'admin/categories',
  component: CategoryListComponent,
  canActivate: [AuthGuard]
},

{
  path: 'admin/categories/add',
  component: AddCategoryComponent,
  canActivate: [AuthGuard]
},
{
  path: 'admin/categories/edit/:id',
  component: EditCategoryComponent,
  canActivate: [AuthGuard]
},
{
  path: 'admin/blogposts',
  component: BlogpostListComponent,
  canActivate: [AuthGuard]
},
{
  path: 'admin/blogposts/add',
  component: AddBlogpostComponent,
  canActivate: [AuthGuard]
},
{
  path: 'admin/blogposts/edit/:id',
  component: EditBlogpostComponent,
  canActivate: [AuthGuard]
},
{
  path: 'login',
  component: LoginComponent
},
{
  path: "whatAreUDoingHere",
  component: UnauthorizedComponent
}





];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
