import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { GiftsManagmentComponent } from './components/gifts-managment/gifts-managment.component';
import { ImageModule } from 'primeng/image';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TooltipModule } from 'primeng/tooltip';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { FloatLabelModule } from 'primeng/floatlabel';
import { DividerModule } from 'primeng/divider';
import { NgModel } from '@angular/forms';
import { ColumnFilter } from 'primeng/table';
import { TableModule } from 'primeng/table';
import { DonatorComponent } from './components/donator/donator.component';


@NgModule({
  declarations: [
    AppComponent,
    GiftsManagmentComponent
  ],
  imports: [
    TableModule,
    ColumnFilter,
    NgModel,
    DividerModule,
    InputTextModule,
    FloatLabelModule,
    CommonModule,
    TooltipModule,
    ConfirmDialogModule,
    ConfirmPopupModule,
    ToastModule,
    ButtonModule,
    BrowserModule,
    ImageModule,
    HttpClientModule,
    CommonModule
  ],
  providers: [],
  bootstrap: [AppComponent, DonatorComponent]
})
export class AppModule { }
