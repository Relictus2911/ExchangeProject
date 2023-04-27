import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable, Subject, map, startWith, switchMap } from 'rxjs';
import { ExchangeService, ICurrency, IExchangeRate } from '../exchange/exchange.service';

@Component({
    selector: 'app-exchange-form',
    templateUrl: './exchange-form.component.html',
    styleUrls: ['./exchange-form.component.css']
})
export class ExchangeFormComponent implements OnInit {

    exchangeForm = new FormGroup({
        currency: new FormControl<ICurrency | null>(null),
        date: new FormControl<Date>(new Date())
    });

    constructor(
        private exchangeService: ExchangeService
    ) { }

    options$!: Observable<ICurrency[]>;
    rate$!: Observable<IExchangeRate>;

    firstAvailableDate$!: Observable<Date>;
    lastAvailableDate$!: Observable<Date>;

    init: boolean = false;

    ngOnInit() {

        this.firstAvailableDate$ = this.exchangeService.getFirstExistingDate();
        this.lastAvailableDate$ = this.exchangeService.getLastxistingDate();
        this.options$ = this.exchangeService.getAllCurrencies();
        this.exchangeForm.valueChanges.subscribe((values) => {
            if (values.currency && values.date) {
              this.rate$ = this.exchangeService.getSelectedExchangeRate(values.currency.code, values.date)
              this.init = true;
            }
        });
    }
}
