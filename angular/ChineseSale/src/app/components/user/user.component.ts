import { Component } from '@angular/core';
import { PaymentComponent } from '../payment/payment.component';
import { DataViewModule } from 'primeng/dataview';
import { ServiceService } from '../../services/gifts/gifts.service';
import { inject } from '@angular/core';
import { Gift } from '../../models/gift.model';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import { MessageService } from 'primeng/api';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { TagModule } from 'primeng/tag';
import { NgClass } from '@angular/common';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ButtonModule } from 'primeng/button';
import { DonatorsService } from '../../services/donators/donators.service';
import { NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TooltipModule } from 'primeng/tooltip';
import { TableModule } from 'primeng/table';
import { IconField } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService } from 'primeng/api';
import { Table } from 'primeng/table';
import { TableService } from 'primeng/table';
import { UserService } from '../../services/user/user.service';
import { ActivatedRoute } from '@angular/router';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Purchase } from '../../models/puchase.model';
import { DialogModule } from 'primeng/dialog';
@Component({
  selector: 'app-user',
  standalone: true,
  imports: [DialogModule, PaymentComponent, CommonModule, RouterOutlet, TableModule, ToastModule, ConfirmPopupModule, InputIconModule, IconField, TooltipModule, FormsModule, NgFor, ButtonModule, SelectButtonModule, NgClass, TagModule, HttpClientModule, DataViewModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  providers: [UserService, TableService, Table, DonatorsService, HttpClientModule, MessageService, ServiceService, ConfirmationService, ServiceService, HttpClient, DonatorsService]
})
export class UserComponent {
  constructor(private router: Router, private messegeService: MessageService) { }

  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  giftService: ServiceService = inject(ServiceService);
  donatorService: DonatorsService = inject(DonatorsService);
  userService: UserService = inject(UserService);
  basketVisible: boolean = false; // משתנה המעקב אחרי הצגת סל הקניות
  vi: boolean = false;
  winnersList: Purchase[] = [];
  giftList: Gift[] = [];

  layout: 'list' | 'grid' = 'grid';  // הגדרת layout עם סוג קבוע

  options: any[] = [
    { label: 'Grid', value: 'grid' },
    { label: 'List', value: 'list' }
  ];


  ngOnInit() {
    this.selectGiftList();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      if (event.url === this.router.url) {
        this.selectGiftList(); // טען מחדש את רשימת המתנות
        this.basketVisible = false;
      }
    });
  }

  selectGiftList() {
    try {
      this.giftService.getAllGifts().subscribe((data: any) => {
        this.giftList = data;
        this.giftList.forEach((gift: any) => {
          console.log(gift.picture);
        });
      });
    } catch {
      console.log("not athourizied");
      this.messegeService.add({ severity: 'error', summary: 'Error', detail: 'משתמש לא מורשה!' });
    }
    console.log(this.giftList);
  }

  logOut() {
    localStorage.clear();
    this.router.navigate(['login'])
  }

  getSeverity(product: any): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    return 'danger';
  }

  async addToCart(giftId: number) {
    var res = await this.userService.addToCart(giftId, 1);
    console.log("res: " + res)
    if (res === 200) {
      this.messegeService.add({ severity: 'success', summary: 'Add', detail: 'מתנה נוספה לסל בהצלחה!' });
    } else {
      this.messegeService.add({ severity: 'error', summary: 'Error', detail: 'מתנה לא נוספה לסל !' });
    }
  }

  async showBaskate() {
    await setTimeout(() => {
      this.basketVisible = true;
    }, 1);
    this.router.navigate(['baskate'], { relativeTo: this.activatedRoute })
  }

  async winners() {
    var f = this.isRandomed();
    if (f === true) {
      await this.getWinners();
    }
    else {
      this.messegeService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'לא כל המתנות הוגרלו!' });
    }
  }

  isRandomed(): boolean {
    for (let index = 0; index < this.giftList.length; index++) {
      if (this.giftList[index].status === true)
        return false;
    }
    return true;
  }

  async getWinners() {
    try {
      this.userService.getWinners().subscribe((data: any) => {
        this.winnersList = data;
        console.log(this.winnersList);
        if (!this.vi) {
          this.vi = true;
        }
      });
    } catch {
      console.log("not athourizied");
      this.messegeService.add({ severity: 'warn', summary: 'Unathourizied', detail: 'משתמש לא מורשה!' });
    }
  }
}
