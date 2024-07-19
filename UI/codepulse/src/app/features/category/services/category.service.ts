import { Injectable } from '@angular/core';
import { AddCategoryRequest } from '../models (Objects)/add-category-request.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models (Objects)/category.model';
import { environment } from 'src/environments/environment';
import { UpdateCategoryRequest } from '../models (Objects)/update-category-request.model copy';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient, private cookieService: CookieService) { }

  addCategory(model: AddCategoryRequest): Observable<void> {

    return this.http.post<void>(`${environment.apiBaseUrl}/api/categories?addAuth=true`, model);

  }

  getAllCategories(): Observable<Category[]> {

    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/Categories`);
  }

  getCategoryById(id: string | null): Observable<Category> {

    //DEVUELVE UN OBSERVABLE CON LA RESPUESTA HTTP DE LA API (O SEA CON EL VALOR)
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`);

  }

  UpdateCategory(id: string, updateCategoryRequest: UpdateCategoryRequest):
    Observable<Category> {

    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`,
      updateCategoryRequest);
  }

  // EL SERVICIO LO QUE HACE ES DEVOLVER LA RESPUESTA DE LA API, EN ESTE CASO LE ESTAMOS ENVIANDO AL 
  // METODO DELETE DE LA API ESTA ID.
  // LA API DEVOLVERA LA RESPUESTA A SU ACCION DENTRO DE UN OBSERVABLE, Y ESO DEBE DE DEVOLVER EL 
  // SERVICIO, PARA QUE EL METODO DEL FRONTEND PUEDA CONSEGUIR LA RESPUESTA (AL SUBSCRIBIRSE A EL)

  deleteCategory(id: string): Observable<Category> {


    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`);
  }

}
