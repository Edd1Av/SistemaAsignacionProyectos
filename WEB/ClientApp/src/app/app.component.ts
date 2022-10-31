import { Component } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from './interfaces/IUsuario';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'app';
  constructor(private authService: AuthorizeService) { }

  usuarioLoggeado:Boolean;

  ngOnInit(){
    this.usuarioLoggeado = this.authService.isLogged();
    this.authService.changeLoginStatus.subscribe((value)=>{
      if(value){
        this.usuarioLoggeado=true;
      }
      else{
        this.usuarioLoggeado = false;
      }
      
    })
  }
}
