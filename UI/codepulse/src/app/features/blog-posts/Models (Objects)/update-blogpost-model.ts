import { Category } from "../../category/models (Objects)/category.model";

export interface UpdateBlogPost
{
   title: string;
   urlHandle: string
   shortDescription: string;
   content: string;
   featuredImageUrl: string;
   publishedDate: Date;
   author: string;
   isVisible: boolean;
   categories: string[];

}