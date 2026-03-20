export interface TransactionDTO {
  id: number;
  gateNumber: string;
  stationName: string;
  cardNumber: string;
  fareAmount: number;
  transactionTime: string;
  paymentType: string;
}
