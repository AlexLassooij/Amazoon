using System;
using System.Threading;
using mongoTest.Models;
using System.Windows.Forms;

namespace mongoTest.Components
{
    public class ShippingTruck : Truck
    {
        public ShippingTruck(Warehouse assignedWarehouse, int initPositionX, int initPositionY, Semaphore TruckSemaphore) :
            base(assignedWarehouse, initPositionX, initPositionY, TruckSemaphore)
        { }

        override
        public void RunTruck()
        {
            NotifyArrival();
            Dock truckDock;
            while ((truckDock = FindAvailableDock()) == null)
            {
                Console.WriteLine($"Truck {Id} waiting for available dock");
                WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
                {
                    WarehouseGUI.Components.Reference_Computer.CurrentForm.updateTruckStatus(
                        Id, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[PositionX] + (PositionY + 1).ToString(),
                        $"Waiting for available dock", numericID);
                });
                Thread.Sleep(500);
            }

            MoveTruckToDockingStation(truckDock);            
            ReadyForLoading();            
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
            foreach (Item item in LoadedItems) {
                AssignedWarehouse.getComputer().inventory.Remove(item);
                foreach (ItemLocation location in AssignedWarehouse.getComputer().inventoryLocations) {
                    if (location.items.Contains(item.Id)) {
                        location.items.Remove(item.Id);
                    }
                }
                
            }
            AssignedWarehouse.GetShippingTrucks().Remove(this);
            TruckState = TruckState.Departed;
            Dock.setDockState(DockState.Available);
            TruckSemaphore.Release();
            Console.WriteLine($"Shipping truck {Id} has left the warehouse");
            WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
            {
                WarehouseGUI.Components.Reference_Computer.CurrentForm.updateTruckStatus(
                    Id, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[PositionX] + (PositionY + 1).ToString(),
                    $"Shipping truck has left the warehouse", numericID);
            });
        }

        override
        public void NotifyArrival()
        {
            // let the computer know that truck has arrived           
            TruckState = TruckState.Arrived;
            Console.WriteLine($"Shipping truck {Id} has arrived and is currently at X: {PositionX} Y: {PositionY}");
            WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
            {
                WarehouseGUI.Components.Reference_Computer.CurrentForm.updateTruckStatus(
                    Id, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[PositionX] + (PositionY + 1).ToString(),
                    $"Shipping truck has arrived and is currently at X: {PositionX} Y: {PositionY}", numericID);
            });
        }

        public void ReadyForLoading()
        {
            Console.WriteLine($"Shipping truck is ready for loading.");
            WarehouseGUI.Components.Reference_Computer.CurrentForm.Invoke((MethodInvoker)delegate
            {
                WarehouseGUI.Components.Reference_Computer.CurrentForm.updateTruckStatus(
                    Id, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()[PositionX] + (PositionY + 1).ToString(),
                    $"Shipping truck is ready for loading.", numericID);
            });
            TruckState = TruckState.Loading;

        }
    }    
}
