import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesRealInsertComponent } from './asignaciones-real-insert.component';

describe('AsignacionesRealInsertComponent', () => {
  let component: AsignacionesRealInsertComponent;
  let fixture: ComponentFixture<AsignacionesRealInsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesRealInsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesRealInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
