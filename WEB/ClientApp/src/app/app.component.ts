import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { IUsuario } from './interfaces/IUsuario';
import { LoaderService } from './loader/loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'app';
  constructor(private authService: AuthorizeService,
    public loaderService:LoaderService,
    private cdRef:ChangeDetectorRef) { }

  usuarioLoggeado:Boolean;
  load:Boolean;

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

  ngAfterViewChecked(){
    this.loaderService.isLoading.subscribe((value)=>{
      this.load=value;
    });
    this.cdRef.detectChanges();
  }
}
