import { Gift } from "./gift.model";

export class Donator {
    id!: number;
    name!: string;
    phone!: string;
    mail!: string;
    address!: string;
    gifts!: Gift[];
}