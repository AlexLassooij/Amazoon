using System;
namespace mongoTest.Components
{
    public class Dock
    {
        public int DockID;
        public int positionX;
        public int positionY;
        public DockState dockState;

        public Dock(int DockID, int positionX, int positionY, DockState dockState) 
        
        {
            
            this.DockID = DockID;
            this.positionX = positionX;
            this.positionY = positionX;
            this.dockState = dockState;
        }

        public bool isAvailable()
        {
            return dockState == DockState.Available;           
        }

        public DockState getDockState()
        {
            return dockState;
        }

        public void setDockState(DockState state)
        {
            dockState = state;
        }


    }
}
