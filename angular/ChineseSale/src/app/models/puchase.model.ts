import { Gift } from "./gift.model";
import { User } from "./user.model";
export class Purchase{
    id!: number;
    giftId!: number;
    customerId!: number;
    status!: boolean;
    ammount!: number;
    totalPrice!: number;
    gift!: Gift;
    customer!: User;
}