import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ExchangeFormComponent } from '../exchange-form/exchange-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
//Change on MomentJS
import { MatNativeDateModule } from '@angular/material/core';
import { ExchangeService } from './exchange.service';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';

@NgModule({
    declarations: [
        ExchangeFormComponent
    ],
    providers: [
        ExchangeService,
        DatePipe
    ],
    imports: [
        CommonModule,
        MatFormFieldModule,
        MatDatepickerModule,
        FormsModule,
        ReactiveFormsModule,
        MatInputModule,
        MatNativeDateModule,
        MatSelectModule,
      MatCardModule
    ],
    exports: [
        ExchangeFormComponent
    ]
})
export class ExchangeModule { }
