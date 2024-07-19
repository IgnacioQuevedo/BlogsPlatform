import { Category } from "../../category/models (Objects)/category.model";

export interface BlogPost
{
    //TIENEN QUE TENER EL MISMO NOMBRE QUE EN LA API, SINO VA A CRASHEAR
    id: string;
    title: string;
    urlHandle: string
    shortDescription: string;
    content: string;
    featuredImageUrl: string;
    publishedDate: Date;
    author: string;
    isVisible: boolean;
    categories: Category[];
    


}