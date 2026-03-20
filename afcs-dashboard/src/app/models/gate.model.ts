export interface GateDTO {
  id: number;
  stationId: number;
  stationName: string;
  gateNumber: string;
  status: string; // open | closed | fault
}
