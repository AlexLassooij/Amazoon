﻿using System;
using System.Threading;
using System.Collections.Generic;
using mongoTest.Models;

namespace mongoTest.Components
{
    public class RestockTruck : Truck
    {
        public RestockTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY, List<Item> restockItems, Semaphore TruckSemaphore) :
            base(assignedWarehouse, initPositionX, initPositionY, TruckSemaphore)
        {
            LoadedItems = new List<Item>(restockItems);
        }

        // after this block is executed, we have whether the truck has 
        // successfully done the following things:
        // 1. notified arrival
        // 2. found an available dock
        // 3. reserved the said available dock
        // 4. moved to the said available dock
        // 5. and is docked
        //

        // truck is spawned
        // notifies arrival
        // waits for available dock
        // moves to dock
        // notifies computer that it is ready for unloading
        // pushes an unload task to queue
        // wait for all items to be unloaded (item list count == 0)
        // leaves warehouse (changes state to departed, frees dock)
        override
        public void RunTruck()
        {
            NotifyArrival();            
            while ((this.Dock = FindAvailableDock()) == null)
            {
                Console.WriteLine($"Truck {Id} waiting for available dock");
                Thread.Sleep(500);
            }
                    
            MoveTruckToDockingStation(this.Dock);            
            ReadyForUnloading();
            AssignedWarehouse.getComputer().CreateUnloadTask(LoadedItems, this);            

            while (LoadedItems.Count > 0)
            {
                Console.WriteLine($"Truck {Id} is being unloaded\n " +
                $"{LoadedItems.Count} more items are still in truck");
                Thread.Sleep(1000);
            }
            LeaveDock();
        }

        override
        public void NotifyDocking()
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Docked;
            Thread.Sleep(500);
        }

        override
        public void LeaveDock() {
            AssignedWarehouse.GetRestockTrucks().Remove(this);
            TruckState = TruckState.Departed;
            Dock.setDockState(DockState.Available);
            Console.WriteLine($"Restock truck {Id} has left the warehouse");
            TruckSemaphore.Release();
        }

        override
        public void NotifyArrival()       
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Arrived;
            Console.WriteLine($"Restock truck {Id} has arrived and is currently at X: {PositionX} Y: {PositionY}");
        }
        

        public void ReadyForUnloading()
        {
            Console.WriteLine($"Restock truck is carrying {GetCurrentWeight()}kg worth of products, and is ready for unloading.");
            TruckState = TruckState.Unloading;
        }

        private void GetRestockTruckLocation()
        {
            Console.WriteLine($"Restock truck at X: {PositionX} Y: {PositionY}");
        }        
    }
}