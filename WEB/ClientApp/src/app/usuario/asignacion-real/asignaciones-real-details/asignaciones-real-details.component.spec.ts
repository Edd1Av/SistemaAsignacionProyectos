import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionesRealDetailsComponent } from './asignaciones-real-details.component';

describe('AsignacionesRealDetailsComponent', () => {
  let component: AsignacionesRealDetailsComponent;
  let fixture: ComponentFixture<AsignacionesRealDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionesRealDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionesRealDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
