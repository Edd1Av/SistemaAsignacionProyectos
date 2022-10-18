import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesRealUpdateComponent } from './asignaciones-real-update.component';

describe('AsignacionesRealUpdateComponent', () => {
  let component: AsignacionesRealUpdateComponent;
  let fixture: ComponentFixture<AsignacionesRealUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesRealUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesRealUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
