import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { AuthService } from "../services/auth.service";
import { jwtDecode } from 'jwt-decode'

export const AuthGuard: CanActivateFn = (route, state) => {

  const cookieService = inject(CookieService);
  const authService = inject(AuthService);
  const router = inject(Router);
  const user = authService.getUser();

  //check for the JWT Token
  let token = cookieService.get('Authorization');

  if (token && user) {
    token = token.replace('Bearer ', '')
    const decodedToken: any = jwtDecode(token);

    //Check if token is expired

    const expirationDate = decodedToken.exp * 1000
    const currentTime = new Date().getTime();

    if (expirationDate < currentTime) {
      //Is expired :C
      // Logout
      authService.logout();
      //Los devuelve a donde estaban antes de volver a la pagina de login
      return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
    }
    else {
      //Token is still valid

      if(user.roles.includes('Writer')){
        return true;
      }
      else{
        router.navigateByUrl('/whatAreUDoingHere?')
        return false;
      }
      
    }
  }
  else {
    // Logout
    authService.logout();
    //Los devuelve a donde estaban antes de volver a la pagina de login
    return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } })
  }

}