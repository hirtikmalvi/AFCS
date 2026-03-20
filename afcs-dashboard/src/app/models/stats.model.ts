export interface StatsDTO {
  totalTransactions: number;
  totalRevenue: number;
  averageFare: number;
  activeGates: number;
  closedGates: number;
  faultGates: number;
  hourlyRevenue: HourlyRevenueDTO[];
  paymentBreakdown: PaymentBreakdownDTO[];
}

export interface HourlyRevenueDTO {
  hour: number;
  revenue: number;
}

export interface PaymentBreakdownDTO {
  paymentType: string;
  count: number;
}
