import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { tap } from 'rxjs/operators';
import { IResponse } from 'src/app/interfaces/IResponse';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-add-administrador',
  templateUrl: './add-administrador.component.html',
  styleUrls: ['./add-administrador.component.css']
})
export class AddAdministradorComponent implements OnInit {

  constructor(
    @Inject(MAT_DIALOG_DATA) private data:any,
    private matDialogref: MatDialogRef<AddAdministradorComponent>,
    private authorizeService: AuthorizeService,
    private _snackBar: MatSnackBar,
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
          this.matDialogref.close();
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
