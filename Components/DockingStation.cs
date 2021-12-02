using System;
using System.Threading;

namespace mongoTest.Components
{
    public class DockingStation
    {
        private int numberOfDockingStations {get;}
        public DockState[] dockStates; // for now let there be two docking stations available
        private int dockingStationLateralPosition; // for now don't worry about this
        
        private Dock[] docks; 


        // DockingStation constructor
        public DockingStation(int numberOfDockingStations)
        {
            this.numberOfDockingStations = numberOfDockingStations;
            this.dockStates = new DockState[this.numberOfDockingStations];

            // initialize every docking station as status available for docking
            for (int i = 0; i < this.numberOfDockingStations; i ++)
            {
                this.dockStates[i] = DockState.Available;
            }
            
        }

        

    }

}

