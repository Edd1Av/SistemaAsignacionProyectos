import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { IResponse } from 'src/app/interfaces/IResponse';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  constructor(private authorizeService: AuthorizeService,
    private activatedRoute: ActivatedRoute,
    private router: Router, private _snackBar: MatSnackBar,
    private formBuilder: FormBuilder) { }

  formGroup!: FormGroup;
  async ngOnInit() {
    this.buildForm();

    
  }

  buildForm(){
    this.formGroup = this.formBuilder.group({
      email: new FormControl("", Validators.required),
    });
  }

  onSubmit(){
    if(this.formGroup.valid){
      this.authorizeService.ResetPassword(this.formGroup.value).pipe(tap((result: IResponse)=>{
        this.openSnackBar(result.response);
        if(result.success){
          this.router.navigate(["/login"]);
        }
      })).subscribe();
    }
    else{
      this.openSnackBar("Ingrese un correo electrónico");
    }
    
  }

  openSnackBar(message:string) {
    this._snackBar.open(message, undefined, {
      duration: 2000,
    });
  }

}
