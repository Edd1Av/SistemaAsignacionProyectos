import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProyectosInsertComponent } from './proyectos-insert.component';

describe('ProyectosInsertComponent', () => {
  let component: ProyectosInsertComponent;
  let fixture: ComponentFixture<ProyectosInsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProyectosInsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProyectosInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
