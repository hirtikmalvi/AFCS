import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { TransactionDTO } from '../../models/transaction.model';
import { GateDTO } from '../../models/gate.model';
@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  // Streams — dashboard subscribes to these

  newTransaction$ = new Subject<TransactionDTO>();
  gateChanged$ = new Subject<GateDTO>();

  private connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7118/hubs/fare')
    .withAutomaticReconnect()
    .build();

  async start() {
    // Listen BEFORE starting connection
    this.connection.on('NewTransaction', (data: TransactionDTO) =>
      this.newTransaction$.next(data),
    );

    this.connection.on('GateStatusChanged', (data: GateDTO) =>
      this.gateChanged$.next(data),
    );

    try {
      await this.connection.start();
      console.log('SignalR connected');
    } catch (err) {
      console.error('SignalR error:', err);
    }
  }
}
