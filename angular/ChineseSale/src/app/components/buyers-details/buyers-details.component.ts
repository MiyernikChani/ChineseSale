import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { DialogModule } from 'primeng/dialog';
import { ServiceService } from '../../services/gifts/gifts.service';
import { inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { Purchase } from '../../models/puchase.model';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';
import { Gift } from '../../models/gift.model';

@Component({
  selector: 'app-buyers-details',
  standalone: true,
  imports: [NgIf, NgFor, ButtonModule, DialogModule, HttpClientModule],
  templateUrl: './buyers-details.component.html',
  styleUrl: './buyers-details.component.css',
  providers: [ServiceService, HttpClientModule, HttpClient]
})
export class BuyersDetailsComponent {
  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }
  giftService: ServiceService = inject(ServiceService);
  visible: boolean = true;
  giftId: number = 0;
  purchaseList: Purchase[] = [];
  gift: Gift | null = null;
  noPuchases: string = "אין רכישות למתנה זו";

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      console.log("Route params: ", params);
      if (params['giftId'] && params['giftId'] > 0) {
        this.giftId = params['giftId'];
        console.log("Gift ID on init: ", this.giftId);
        this.getAllPurchasesByGiftId();
      }
    });
  }

  getAllPurchasesByGiftId() {
    this.giftService.getAllPurchasesByGiftId(this.giftId).subscribe(
      (data) => {
        console.log("purchases list: ");
        this.purchaseList = data;
        console.log(this.purchaseList);
        this.visible = true;
        this.gift = this.purchaseList[0].gift;
      },
      (error) => {
        console.log("error", error);
      }
    );
  }

  // getGiftById() {
  //   this.giftService.getGiftById(this.giftId).subscribe(
  //     (data) => {
  //       if (data) {
  //         this.gift = data;
  //         console.log("Gift loaded:", this.gift);
  //       } else {
  //         console.log("Gift not found");
  //       }
  //     },
  //     (error) => {
  //       console.error("Error loading gift:", error);
  //     }
  //   );
  // }

  close() {
    this.visible = false;
    this.router.navigate(['/admin']);
  }
}
