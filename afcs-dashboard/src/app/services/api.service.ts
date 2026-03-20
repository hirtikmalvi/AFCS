import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { TransactionDTO } from '../models/transaction.model';
import { Result } from '../models/result.model';
import { GateDTO } from '../models/gate.model';
import { StatsDTO } from '../models/stats.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private BASE_URL = 'https://localhost:7118/api';

  private http = inject(HttpClient);

  getTransactions(): Observable<TransactionDTO[]> {
    return this.http
      .get<Result<TransactionDTO[]>>(`${this.BASE_URL}/transaction`)
      .pipe(map((r) => r.data));
  }
  getGates(): Observable<GateDTO[]> {
    return this.http
      .get<Result<GateDTO[]>>(`${this.BASE_URL}/api/gate`)
      .pipe(map((r) => r.data));
  }

  getStats(): Observable<StatsDTO> {
    return this.http
      .get<Result<StatsDTO>>(`${this.BASE_URL}/api/stats/summary`)
      .pipe(map((r) => r.data));
  }
}
