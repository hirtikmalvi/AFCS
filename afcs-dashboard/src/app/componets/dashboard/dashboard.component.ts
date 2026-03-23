import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { StatsDTO } from '../../models/stats.model';
import { GateDTO } from '../../models/gate.model';
import { TransactionDTO } from '../../models/transaction.model';
import { Subscription } from 'rxjs';
import { ApiService } from '../../services/api.service';
import { SignalRService } from '../../services/signal-r/signal-r.service';
import { HideCardNumberPipe } from '../../pipes/hide-card-number.pipe';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, HideCardNumberPipe],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('barCanvas') barRef!: ElementRef;
  @ViewChild('donutCanvas') donutRef!: ElementRef;

  stats: StatsDTO | null = null;
  gates: GateDTO[] = [];
  transactions: TransactionDTO[] = [];

  connected = false;

  private barChart!: Chart;
  private donutChart!: Chart;
  private subs: Subscription[] = [];

  private api = inject(ApiService);
  private hub = inject(SignalRService);

  async ngOnInit() {
    this.loadAll();

    await this.hub.start();
    this.connected = true;

    this.subs.push(
      this.hub.newTransaction$.subscribe((t) => this.onNewTransaction(t)),
      this.hub.gateChanged$.subscribe((g) => this.onGateChanged(g)),
    );
  }
  ngAfterViewInit(): void {
    this.initBarChart();
    this.initDonutChart();
  }
  ngOnDestroy(): void {
    this.subs.forEach((s) => s.unsubscribe());
    this.barChart?.destroy();
    this.donutChart?.destroy();
  }

  loadAll() {
    this.api.getStats().subscribe((s) => {
      this.stats = s;
      this.updateBarChart(s);
      this.updateDonutChart(s);
    });

    this.api.getGates().subscribe((g) => {
      this.gates = g;
    });

    this.api.getTransactions().subscribe((t) => {
      this.transactions = t;
    });
  }

  // SignalR Handlers
  onNewTransaction(t: TransactionDTO) {
    // Prepend to transaction feed (keep max 50)
    this.transactions = [t, ...this.transactions].slice(0, 20);

    this.api.getStats().subscribe((s) => {
      if (s) {
        this.stats = s;
      }
    });

    // Update stat cards
    if (this.stats) {
      // Update hourly bar chart
      const hour = new Date(t.transactionTime).getHours();
      const existing = this.stats.hourlyRevenue.find((h) => h.hour === hour);
      if (existing) {
        existing.revenue = +(existing.revenue + t.fareAmount).toFixed(2);
      } else {
        this.stats.hourlyRevenue.push({ hour, revenue: t.fareAmount });
        this.stats.hourlyRevenue.sort((a, b) => a.hour - b.hour);
      }

      // Update payment breakdown donut
      const pay = this.stats.paymentBreakdown.find(
        (p) => p.paymentType === t.paymentType,
      );
      if (pay) {
        pay.count++;
      } else {
        this.stats.paymentBreakdown.push({
          paymentType: t.paymentType,
          count: 1,
        });
      }

      // Refresh both charts
      this.updateBarChart(this.stats);
      this.updateDonutChart(this.stats);
    }
  }

  onGateChanged(g: GateDTO) {
    // Update the gate in the grid
    const idx = this.gates.findIndex((x) => x.id === g.id);
    if (idx > -1) this.gates[idx] = g;

    // Refresh gate counts in stats
    this.api.getStats().subscribe((s) => {
      if (this.stats) {
        this.stats.activeGates = s.activeGates;
        this.stats.closedGates = s.closedGates;
        this.stats.faultGates = s.faultGates;
      }
    });
  }

  // Chart Init
  initBarChart() {
    this.barChart = new Chart(this.barRef.nativeElement, {
      type: 'bar',
      data: {
        labels: [],
        datasets: [
          {
            label: 'Revenue (₹)',
            data: [],
            backgroundColor: 'rgba(56,138,221,0.7)',
            borderRadius: 4,
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            display: false,
          },
        },
        scales: {
          x: { title: { display: true, text: 'Hour' } },
          y: { title: { display: true, text: '₹' }, beginAtZero: true },
        },
      },
    });
  }

  initDonutChart() {
    this.donutChart = new Chart(this.donutRef.nativeElement, {
      type: 'doughnut',
      data: {
        labels: ['Card', 'Cash', 'QR'],
        datasets: [
          {
            data: [0, 0, 0],
            backgroundColor: [
              'rgba(56,138,221,0.8)',
              'rgba(29,158,117,0.8)',
              'rgba(239,159,39,0.8)',
            ],
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { position: 'bottom' },
        },
      },
    });
  }

  // Chart Update
  updateBarChart(s: StatsDTO) {
    if (!this.barChart) return;
    this.barChart.data.labels = s.hourlyRevenue.map((h) => `${h.hour}:00`);
    this.barChart.data.datasets[0].data = s.hourlyRevenue.map((h) => h.revenue);
    this.barChart.update();
  }

  updateDonutChart(s: StatsDTO) {
    if (!this.donutChart) return;
    const order = ['card', 'cash', 'qr'];
    this.donutChart.data.datasets[0].data = order.map((type) => {
      const found = s.paymentBreakdown.find((p) => p.paymentType === type);
      return found ? found.count : 0;
    });
    this.donutChart.update();
  }

  // Helper
  gateClass(status: string): string {
    if (status === 'open') return 'gate-open';
    if (status === 'fault') return 'gate-fault';
    return 'gate-closed';
  }
}
