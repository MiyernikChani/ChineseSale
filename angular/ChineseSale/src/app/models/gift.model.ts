import { Category } from "./category.model";
export class Gift{
    id!:number;
    name!:string;
    donatorId!:number;
    price!:number;
    countOfSales!:number;
    picture!:string;
    status!:boolean;
    categoryId!:number;
    category!:Category;
}