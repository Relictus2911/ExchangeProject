import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, map } from "rxjs";

export interface ICurrency {
    amount: number;
    code: string;
}

export interface IExchangeRate {
    id: string;
    rate: string;
    currency: ICurrency;
    exchangeDate: ExchangeDate;
}

interface ExchangeDate {
    date: Date;
}

@Injectable()
export class ExchangeService {

    constructor(
        private httpClient: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string) {
    }

    getAllCurrencies(): Observable<ICurrency[]> {
        return this.httpClient.get<ICurrency[]>(this.baseUrl + 'exchangerate');
    }

    getSelectedExchangeRate(code: string, date: Date): Observable<IExchangeRate> {
      // date.getMonth() returns values form 0 to 11(this is just how it works). In real project we use library for that or just write custom dateTransformer
      const transformedDate = `${date.getMonth() + 1}-${date.getDate()}-${date.getFullYear()}`;
      return this.httpClient.get<IExchangeRate>(this.baseUrl + `exchangerate/${code}/${transformedDate}`);
    }

    getFirstExistingDate(): Observable<Date> {
        return this.httpClient.get<Date>(this.baseUrl + 'exchangerate/firstdate');
    }

    getLastxistingDate(): Observable<Date> {
        return this.httpClient.get<Date>(this.baseUrl + 'exchangerate/lastdate');
    }

}
