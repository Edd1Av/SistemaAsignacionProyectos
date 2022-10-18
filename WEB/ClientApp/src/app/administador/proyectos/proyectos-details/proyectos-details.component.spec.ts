import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProyectosDetailsComponent } from './proyectos-details.component';

describe('ProyectosDetailsComponent', () => {
  let component: ProyectosDetailsComponent;
  let fixture: ComponentFixture<ProyectosDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProyectosDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProyectosDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
