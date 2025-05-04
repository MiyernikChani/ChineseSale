import { Component } from '@angular/core';
import { Purchase } from '../../models/puchase.model';
import { UserService } from '../../services/user/user.service';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { NgClass } from '@angular/common';
import { OrderList } from 'primeng/orderlist';
import { NgIf } from '@angular/common';
import { AsyncPipe } from '@angular/common';
import { NgFor } from '@angular/common';
import { DividerModule } from 'primeng/divider';
import { filter } from 'rxjs';
import { NavigationEnd } from '@angular/router';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { PaymentComponent } from '../payment/payment.component';

@Component({
  selector: 'app-baskate',
  standalone: true,
  imports: [PaymentComponent, ButtonModule, RouterOutlet, DividerModule, NgFor, AsyncPipe, NgIf, HttpClientModule, OrderList, NgClass],
  templateUrl: './baskate.component.html',
  styleUrl: './baskate.component.css',
  providers: [MessageService, UserService, HttpClient, HttpClientModule]
})
export class BaskateComponent {
  constructor(private messegeServise: MessageService, private router: Router, private activatedRoute: ActivatedRoute) { }

  userServise: UserService = inject(UserService)
  cartList: Purchase[] = [];
  historyList: Purchase[] = [];
  payment: boolean = true;

  price: number = 0;

  ngOnInit() {
    this.getCartList()
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      if (event.url === this.router.url) {
        this.getCartList()
      }
    });
  }

  getCartList() {
    try {
      this.userServise.showBaskate().subscribe((data: any) => {
        this.cartList = data;
        if (this.cartList != null)
          this.payment = true;
        else
          this.payment = false;
        console.log(this.payment);
        console.log(this.cartList);
        this.gethistoryList();
      });
    } catch {
      console.log("not athourizied");
      this.messegeServise.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
  }

  gethistoryList() {
    try {
      this.userServise.historyShopping().subscribe((data: any) => {
        this.historyList = data;
        console.log(this.historyList);
        this.getPrice();
      });
    } catch {
      console.log("not athourizied");
      this.messegeServise.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
  }

  async updateQuantity(purchase: Purchase, num: number) {
    var res = await this.userServise.addToCart(purchase.giftId, num);
    console.log("res: " + res)
    if (res === 200) {
      console.log("מתנה עודכנה");
      this.getCartList()
      this.getPrice();
    } else {
      console.log("שגיאה בעדכון מתנה");
    }
  }

  async getPrice() {
    try {
      this.userServise.getPrice().subscribe((data: any) => {
        this.price = data;
        console.log("sum: " + this.price);
      });
    } catch {
      console.log("not athourizied");
      this.messegeServise.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
  }

  close() {
    this.router.navigate(['user'])
  }

  async buyCart() {
    this.payment = false;
    // await this.router.navigate(['payment'], { relativeTo: this.activatedRoute });
  }


  handlePaymentEvent(paymentStatus: boolean) {
    this.getCartList();
    this.gethistoryList();
    this.payment = paymentStatus;
  }
}
