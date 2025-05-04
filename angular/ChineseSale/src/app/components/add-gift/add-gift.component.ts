import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { Validators } from '@angular/forms';
import { ServiceService } from '../../services/gifts/gifts.service';
import { inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Gift } from '../../models/gift.model';
import { Category } from '../../models/category.model';
import { DonatorsService } from '../../services/donators/donators.service';
import { Donator } from '../../models/donator.model';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { FloatLabel } from 'primeng/floatlabel';

@Component({
    selector: 'app-add-gift',
    standalone: true,
    imports: [FloatLabel, ButtonModule, DialogModule, ToastModule, NgIf, ReactiveFormsModule, CommonModule],
    templateUrl: './add-gift.component.html',
    styleUrl: './add-gift.component.css',
    providers: [MessageService, ServiceService, DonatorsService]
})
export class AddGiftComponent {
    constructor(private messageService: MessageService) {
        this.frmGift = new FormGroup({
            name: new FormControl('', [Validators.required, Validators.minLength(5), Validators.maxLength(50)]),
            donator: new FormControl(0, Validators.required),
            price: new FormControl(0, [Validators.required, Validators.min(1), Validators.max(100)]),
            category: new FormControl(0, Validators.required)
        })
    }
    visible: boolean = true;
    frmGift: FormGroup;

    defaultPrice: number = 10;
    selectedFile: File | null = null;
    image: string | ArrayBuffer | null = null;

    router: Router = inject(Router);
    activatedRoute: ActivatedRoute = inject(ActivatedRoute);

    donatorService: DonatorsService = inject(DonatorsService);
    giftService: ServiceService = inject(ServiceService);

    giftList: Gift[] = [];
    donatorList: Donator[] = [];
    categoryList: Category[] = [];

    ngOnInit() {
        this.selectGiftList();
    }
    selectGiftList() {
        this.giftService.getAllGifts().subscribe((data: any) => {
            this.giftList = data;
            this.selectDonatorList();
        });
    }

    selectDonatorList() {
        this.donatorService.getAllDonators().subscribe((data: Donator[]) => {
            this.donatorList = data;
            this.selectCategoryList();
        });
    }

    selectCategoryList() {
        this.giftService.getAllCategories().subscribe((data: any) => {
            this.categoryList = data;
            console.log(this.giftList);
        });
    }

    async addGift(event: Event) {
        event.preventDefault();
        var gift = new Gift();
        gift.name = this.frmGift.controls['name'].value;
        gift.donatorId = this.frmGift.controls['donator'].value;
        gift.price = this.frmGift.controls['price'].value;
        gift.countOfSales = 0;
        gift.picture = '';
        gift.status = true;
        gift.categoryId = this.frmGift.controls['category'].value;
        gift.category = new Category();
        gift.category.id = gift.categoryId;
        gift.category.title = '';

        this.giftService.addGift(gift).subscribe({
            next: async response => {
                console.log('סטטוס:', response.status);
                console.log(response.message);
                if (this.selectedFile) {
                    if (response.gift) {
                        console.log(response.gift.id);
                        await this.giftService.updateImage(this.selectedFile, response.gift.id);
                        this.showSuccessToast();
                    } else {
                        this.showErrorToast();
                    }
                }
            },
            error: HttpErrorResponse => {
                console.log(HttpErrorResponse.status);
                if (HttpErrorResponse.status === 400) {
                    this.visible = false;
                    this.messageService.add({ severity: 'error', summary: 'Error', detail: 'כבר קיימת מתנה בשם זה!' });
                    this.close();
                } else {
                    console.log("error");

                    this.showErrorToast();
                }
            }
        });
    }
    async showErrorToast() {
        this.visible = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'המתנה לא נוספה!' });
        this.close();
    }
    async showSuccessToast() {
        this.visible = false;
        this.messageService.add({ severity: 'success', summary: 'add', detail: 'מתנה נוספה בהצלחה!' });
        this.close();
    }

    close() {
        setTimeout(() => {
            this.messageService.clear();
            this.route();
        }, 2000);
    }

    route() {
        setTimeout(() => {
            this.router.navigate(['admin']);
        }, 2000);
    }

    shut(){
        this.router.navigate(['admin']);
    }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.selectedFile = input.files[0];
            const reader = new FileReader();
            reader.onload = () => {
                this.image = reader.result; // שומר את התמונה במשתנה
            };
            reader.readAsDataURL(this.selectedFile);
        }
    }
}
