import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesInsertComponent } from './asignaciones-insert.component';

describe('AsignacionesInsertComponent', () => {
  let component: AsignacionesInsertComponent;
  let fixture: ComponentFixture<AsignacionesInsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesInsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesInsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
