import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProyectosUpdateComponent } from './proyectos-update.component';

describe('ProyectosUpdateComponent', () => {
  let component: ProyectosUpdateComponent;
  let fixture: ComponentFixture<ProyectosUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProyectosUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProyectosUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
