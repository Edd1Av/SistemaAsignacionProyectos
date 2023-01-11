import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteAdministradorComponent } from './delete-administrador.component';

describe('DeleteAdministradorComponent', () => {
  let component: DeleteAdministradorComponent;
  let fixture: ComponentFixture<DeleteAdministradorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteAdministradorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteAdministradorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
