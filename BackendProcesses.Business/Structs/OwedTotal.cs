﻿namespace BackendProcesses.Business.Structs
{
    public struct OwedTotal
    {
        public decimal FeesOwedTotal { get; set; }
        public decimal LumpSumOwedTotal { get; set; }
        public decimal PeriodicPaymentOwedTotal { get; set; }
    }
}
