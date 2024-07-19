import { Component} from '@angular/core';
import { LoginRequest } from '../models/login-request.model';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { CookieService } from 'ngx-cookie-service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent 
{

  loginExample: LoginRequest;

  constructor(private cookieService: CookieService,private router: Router, private authService : AuthService)
  {
    this.loginExample =
    {
      email: '',
      password: ''
    };
  
  }

  onFormSubmit() : void
  {
    this.authService.login(this.loginExample)
    .subscribe({
      next: (response) => {
        // Set Auth Cookie
        this.cookieService.set('Authorization',`Bearer ${response.token}`,
        undefined, '/', undefined, true, 'Strict');
    
        // Set User

        this.authService.setUser({
          email: response.email,
          roles: response.roles
        });

        //Redirect back to Home
        this.router.navigateByUrl('/');
      }
    });
  }


  

}
