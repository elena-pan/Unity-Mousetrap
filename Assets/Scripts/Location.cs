using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

namespace MouseTrap
{
    public class Location
    {
        public string name;
        public Vector3 gridPoint;
        public int numCheese;
        public int buildNum;
        public int moveSteps;
        public int moveTo;

        public Location(string name, int numCheese, int buildNum, int moveSteps, int moveTo)
        {
            this.name = name;
            //this.gridPoint = gridPoint; // Set this up later
            this.numCheese = numCheese;
            this.buildNum = buildNum;
            this.moveSteps = moveSteps;
            this.moveTo = moveTo;
        }
    }
}