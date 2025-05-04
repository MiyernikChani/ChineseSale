import { Routes } from '@angular/router';
import { GiftsManagmentComponent } from './components/gifts-managment/gifts-managment.component';
import { UpdateGiftComponent } from './components/update-gift/update-gift.component';
import { AddGiftComponent } from './components/add-gift/add-gift.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponentComponent } from './components/register/register.component';
import { UserComponent } from './components/user/user.component';
import { BaskateComponent } from './components/baskate/baskate.component';
import { PaymentComponent } from './components/payment/payment.component';
import { DonatorComponent } from './components/donator/donator.component';
import { AddDonatorComponent } from './components/add-donator/add-donator.component';
import { UpdateDonatorComponent } from './components/update-donator/update-donator.component';
import { BuyersDetailsComponent } from './components/buyers-details/buyers-details.component';

export const routes: Routes = [
    {
        path: '', component: LoginComponent
    },
    {
        path: 'admin', component: GiftsManagmentComponent, children: [
            { path: 'update/:id', component: UpdateGiftComponent },
            { path: 'add', component: AddGiftComponent },
            { path: 'buyersDetails/:giftId', component: BuyersDetailsComponent },
            { path: '', redirectTo: 'admin', pathMatch: 'full' },
        ],
    },
    {
        path: 'login', component: LoginComponent
    },
    {
        path: 'register', component: RegisterComponentComponent
    },
    {
        path: 'user', component: UserComponent, children: [
            {
                path: 'baskate', component: BaskateComponent, children: [
                    { path: 'payment', component: PaymentComponent }
                ]
            }
        ]
    },
    {
        path: 'donator', component: DonatorComponent, children: [
            {path: 'addDonator', component: AddDonatorComponent},
            {path: 'updateDonator/:id', component: UpdateDonatorComponent},
            {path: '', redirectTo: 'admin', pathMatch: 'full' },
        ]
    }
]