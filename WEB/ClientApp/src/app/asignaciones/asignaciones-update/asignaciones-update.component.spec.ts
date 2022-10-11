import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesUpdateComponent } from './asignaciones-update.component';

describe('AsignacionesUpdateComponent', () => {
  let component: AsignacionesUpdateComponent;
  let fixture: ComponentFixture<AsignacionesUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesUpdateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
