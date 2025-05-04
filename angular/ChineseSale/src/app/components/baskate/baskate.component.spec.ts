import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaskateComponent } from './baskate.component';

describe('BaskateComponent', () => {
  let component: BaskateComponent;
  let fixture: ComponentFixture<BaskateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaskateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaskateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
