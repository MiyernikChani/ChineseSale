import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { MessageService } from 'primeng/api';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { NgIf } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { futureDateValidator } from './dateValidator';
import { UserService } from '../../services/user/user.service';
import { inject } from '@angular/core';
import { Purchase } from '../../models/puchase.model';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
// import { fontBase64 } from 'src/assets/fonts/font.js';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, ButtonModule, ToastModule, DialogModule, HttpClientModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
  providers: [HttpClient, HttpClientModule]
})
export class PaymentComponent {
  constructor(private router: Router, private messageService: MessageService) {
    this.frmPayment = new FormGroup({
      num: new FormControl('', [Validators.required, Validators.minLength(16), Validators.maxLength(16), Validators.pattern(/^\d+$/)]),
      validity: new FormControl('', [Validators.required, futureDateValidator]),
      tag: new FormControl('', [Validators.required, Validators.minLength(3), Validators.maxLength(3), Validators.pattern(/^\d+$/)]),
      idNumber: new FormControl('', [Validators.maxLength(9), Validators.minLength(9), Validators.pattern(/^\d+$/)])
    })
  }

  @Output() paymentEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  cartList: Purchase[] = [];

  pdfGenerated = false;
  pdfBlobUrl: string | null = null;

  visible: boolean = true;
  frmPayment: FormGroup;
  userService: UserService = inject(UserService)

  ngOnInit() {
    this.getCartList()
  }

  getCartList() {
    try {
      this.userService.showBaskate().subscribe((data: any) => {
        this.cartList = data;
      });
    } catch {
      console.log("not athourizied");
    }
  }

  route() {
    this.paymentEvent.emit(true);
  }

  pay() {
    this.userService.buyCart().subscribe({
      next: async response => {
        console.log('סטטוס:', response.status);
        console.log('תגובה מהשרת:', JSON.stringify(response.body, null, 2));
        // this.createReception();
        this.processPayment()
      },
      error: error => {
        console.error('תשלום נכשל', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'תשלום נכשל!' });
        setTimeout(() => {
          this.route()
        }, 3000);
      }
    });
  }

  isProcessing = false;

  processPayment() {
    this.isProcessing = true;

    setTimeout(() => {
      this.isProcessing = false; // עצירת האנימציה אחרי 3 שניות
      this.messageService.add({ severity: 'success', summary: 'success', detail: 'תשלום בוצע בהצלחה!' });
      setTimeout(() => {
        this.route()
      }, 3000);
    }, 3000);
  }


  async getPrice() {
    var price = 0;
    try {
      this.userService.getPrice().subscribe((data: any) => {
        price = data;
      });
    } catch {
      console.log("not athourizied");
    }
    return price;
  }
}
