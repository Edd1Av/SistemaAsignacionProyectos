import { Component } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from '../interfaces/IUsuario';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private _authService:AuthorizeService) 
    { }

    usuario:IUsuario;
    ngOnInit(): void {

      if(this._authService.usuarioData!=null){
        this.usuario=this._authService.usuarioData;
        console.log(this.usuario);
      }
    }
}
