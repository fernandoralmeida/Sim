﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Threading;

namespace Sim.Monitoring.ViewModel
{

    using Model;
    using Mvvm.Helpers.Notifiers;

    class vmDesempenho : NotifyProperty
    {
        private mMemory _momory = new mMemory();
        private Process currentProc = Process.GetCurrentProcess();

        public mMemory Memory
        {
            get { return _momory; }
            set
            {
                _momory = value;
                RaisePropertyChanged("Memory");
            }
        }

        public vmDesempenho()
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                DispatcherTimer dispTimer = new DispatcherTimer();
                dispTimer.Tick += new EventHandler(dispTimer_Tick);
                dispTimer.Interval = new TimeSpan(0, 0, 1);
                dispTimer.Start();
            },
            System.Threading.CancellationToken.None,
            System.Threading.Tasks.TaskCreationOptions.None,
            System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());          
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            Memory.Used = string.Format("Memória: {0:#.#,##}KB {1:#.#,##}MB", GC.GetTotalMemory(true) / 1024, (GC.GetTotalMemory(true) / 1024) / 1024);
            
            Memory.Private = string.Format("Privada: {0:#.#,##}KB {1:#.#,##}MB", currentProc.PrivateMemorySize64 / 1024, (currentProc.PrivateMemorySize64 / 1024) / 1024);

            Memory.Pagined = string.Format("Paginada: {0:#.#,##}KB {1:#.#,##}MB", currentProc.PagedMemorySize64 / 1024, (currentProc.PagedMemorySize64 / 1024) / 1024);
            Memory.PeakPagined = string.Format("Pico: {0:#.#,##}KB {1:#.#,##}MB", currentProc.PeakPagedMemorySize64 / 1024, (currentProc.PeakPagedMemorySize64 / 1024) / 1024);

            Memory.Physic = string.Format("Fisica: {0:#.#,##}KB {1:#.#,##}MB", currentProc.WorkingSet64 / 1024, (currentProc.WorkingSet64 / 1024) / 1024);
            Memory.PeakPhysic = string.Format("Pico: {0:#.#,##}KB {1:#.#,##}MB", currentProc.PeakWorkingSet64 / 1024, (currentProc.PeakWorkingSet64 / 1024) / 1024);

            Memory.Virtual = string.Format("Virtual: {0:#.#,##}KB {1:#.#,##}MB", currentProc.VirtualMemorySize64 / 1024, (currentProc.VirtualMemorySize64 / 1024) / 1024);
            Memory.PeakVirtual = string.Format("Pico: {0:#.#,##}KB {1:#.#,##}MB", currentProc.PeakVirtualMemorySize64 / 1024, (currentProc.PeakVirtualMemorySize64 / 1024) / 1024);
        }        
    }
}
