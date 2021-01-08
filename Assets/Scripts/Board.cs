using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;

namespace MouseTrap
{
    public class Board : MonoBehaviour
    {
        public Location[] locations;

        // Refactor this to make it easier to read
        public void SetUpLocations()
        {
            this.locations = new Location[50];
            this.locations[0] = new Location("Start", 0, 0, 0, -1);
            this.locations[1] = new Location("Build", 0, 2, 0, -1);
            this.locations[2] = new Location("Take 2 Pieces of Cheese", 2, 0, 0, -1);
            this.locations[3] = new Location("Build", 0, 4, 0, -1);
            this.locations[4] = new Location("Build", 0, 2, 0, -1);
            this.locations[5] = new Location("Cat's Meow - Go Back 2 Spaces", 0, 0, -2, -1);
            this.locations[6] = new Location("Build", 0, 4, 0, -1);
            this.locations[7] = new Location("Take 1 Piece of Cheese", 1, 0, 0, -1);
            this.locations[8] = new Location("Build", 0, 4, 0, -1);
            this.locations[9] = new Location("Build", 0, 3, 0, -1);
            this.locations[10] = new Location("Move Ahead 4 Spaces", 0, 0, 4, -1);
            this.locations[11] = new Location("Build", 0, 4, 0, -1);
            this.locations[12] = new Location("Build", 0, 2, 0, -1);
            this.locations[13] = new Location("Fat Cat! Go Back to Start", 0, 0, 0, 0);
            this.locations[14] = new Location("Build", 0, 4, 0, -1);
            this.locations[15] = new Location("Go Back 6 Spaces", 0, 0, -6, -1);
            this.locations[16] = new Location("Build", 0, 2, 0, -1);
            this.locations[17] = new Location("Build", 0, 4, 0, -1);
            this.locations[18] = new Location("Build", 0, 3, 0, -1);
            this.locations[19] = new Location("Build", 0, 2, 0, -1);
            this.locations[20] = new Location("Build", 0, 4, 0, -1);
            this.locations[21] = new Location("Dog Bone", 0, 0, 0, -1);
            this.locations[22] = new Location("Build", 0, 4, 0, -1);
            this.locations[23] = new Location("Build", 0, 2, 0, -1);
            this.locations[24] = new Location("Lose 2 Pieces of Cheese", -2, 0, 0, -1);
            this.locations[25] = new Location("Build", 0, 2, 0, -1);
            this.locations[26] = new Location("Build", 0, 4, 0, -1);
            this.locations[27] = new Location("Mad Dog! Go Back to Dog Bone", 0, 0, 0, 21);
            this.locations[28] = new Location("Build", 0, 4, 0, -1);
            this.locations[29] = new Location("Build", 0, 3, 0, -1);
            this.locations[30] = new Location("Move Ahead to Cheese Wheel", 0, 0, 0, 49);
            this.locations[31] = new Location("Build", 0, 2, 0, -1);
            this.locations[32] = new Location("Build", 0, 4, 0, -1);
            this.locations[33] = new Location("Go Back 4 Spaces", 0, 0, -4, -1);
            this.locations[34] = new Location("Build", 0, 2, 0, -1);
            this.locations[35] = new Location("Build", 0, 4, 0, -1);
            this.locations[36] = new Location("Build", 0, 3, 0, -1);
            this.locations[37] = new Location("Take 3 Pieces of Cheese From the Rival With the Most", 3, 0, 0, -1);
            this.locations[38] = new Location("Build", 0, 4, 0, -1);
            this.locations[39] = new Location("Caught Napping! Lose 3 Pieces of Cheese", -3, 0, 0, -1);
            this.locations[40] = new Location("Build", 0, 4, 0, -1);
            this.locations[41] = new Location("Build", 0, 3, 0, -1);
            this.locations[42] = new Location("Go Back 6 Spaces", 0, 0, -6, -1);
            this.locations[43] = new Location("Build", 0, 2, 0, -1);
            this.locations[44] = new Location("SAFE", 0, 0, 0, -1);
            this.locations[45] = new Location("Build 2", 0, 4, 0, -1);
            this.locations[46] = new Location("TURN CRANK", 0, 0, 0, -1);
            this.locations[47] = new Location("Build 2", 0, 4, 0, -1);
            this.locations[48] = new Location("Take 2 Pieces of Cheese", 2, 0, 0, -1);
            this.locations[49] = new Location("CHEESE WHEEL", 2, 0, 0, -1);

            SetUpGridPoints();
        }

        private void SetUpGridPoints() 
        {
            this.locations[0].gridPoint = new Vector3(11f, 0f, -1f);
            this.locations[1].gridPoint = new Vector3(11f, 0f, 0.5f);
            this.locations[2].gridPoint = new Vector3(11f, 0f, 1.5f);
            this.locations[3].gridPoint = new Vector3(11f, 0f, 3.3f);
            this.locations[4].gridPoint = new Vector3(11f, 0f, 5f);
            this.locations[5].gridPoint = new Vector3(10.5f, 0f, 7f);
            this.locations[6].gridPoint = new Vector3(11f, 0f, 8.5f);
            this.locations[7].gridPoint = new Vector3(9.9f, 0f, 9.7f);
            this.locations[8].gridPoint = new Vector3(8.5f, 0f, 10f);
            this.locations[9].gridPoint = new Vector3(7.5f, 0f, 10f);
            this.locations[10].gridPoint = new Vector3(6f, 0f, 10f);
            this.locations[11].gridPoint = new Vector3(4.7f, 0f, 9.2f);
            this.locations[12].gridPoint = new Vector3(3f, 0f, 10.3f);
            this.locations[13].gridPoint = new Vector3(1.5f, 0f, 10.3f);
            this.locations[14].gridPoint = new Vector3(0f, 0f, 10.3f);
            this.locations[15].gridPoint = new Vector3(-1f, 0f, 10.3f);
            this.locations[16].gridPoint = new Vector3(-2.5f, 0f, 10.5f);
            this.locations[17].gridPoint = new Vector3(-4f, 0f, 10f);
            this.locations[18].gridPoint = new Vector3(-5.5f, 0f, 10.5f);
            this.locations[19].gridPoint = new Vector3(-7f, 0f, 10.5f);
            this.locations[20].gridPoint = new Vector3(-9f, 0f, 10.5f);
            this.locations[21].gridPoint = new Vector3(-9.5f, 0f, 9.5f);
            this.locations[22].gridPoint = new Vector3(-10.5f, 0f, 8f);
            this.locations[23].gridPoint = new Vector3(-10.7f, 0f, 6.7f);
            this.locations[24].gridPoint = new Vector3(-11.5f, 0f, 5f);
            this.locations[25].gridPoint = new Vector3(-10f, 0f, 3.6f);
            this.locations[26].gridPoint = new Vector3(-9f, 0f, 2f);
            this.locations[27].gridPoint = new Vector3(-10.5f, 0f, 0.5f);
            this.locations[28].gridPoint = new Vector3(-9f, 0f, -1f);
            this.locations[29].gridPoint = new Vector3(-9.5f, 0f, -3f);
            this.locations[30].gridPoint = new Vector3(-10f, 0f, -5f);
            this.locations[31].gridPoint = new Vector3(-10.5f, 0f, -6f);
            this.locations[32].gridPoint = new Vector3(-10.5f, 0f, -8f);
            this.locations[33].gridPoint = new Vector3(-10f, 0f, -9.5f);
            this.locations[34].gridPoint = new Vector3(-9f, 0f, -10.3f);
            this.locations[35].gridPoint = new Vector3(-7f, 0f, -10.3f);
            this.locations[36].gridPoint = new Vector3(-5.8f, 0f, -9.9f);
            this.locations[37].gridPoint = new Vector3(-4.5f, 0f, -9.5f);
            this.locations[38].gridPoint = new Vector3(-3f, 0f, -9f);
            this.locations[39].gridPoint = new Vector3(-1f, 0f, -9f);
            this.locations[40].gridPoint = new Vector3(0.5f, 0f, -9.5f);
            this.locations[41].gridPoint = new Vector3(2f, 0f, -10f);
            this.locations[42].gridPoint = new Vector3(4f, 0f, -10f);
            this.locations[43].gridPoint = new Vector3(5.5f, 0f, -10f);
            this.locations[44].gridPoint = new Vector3(8f, 0f, -11f);
            this.locations[45].gridPoint = new Vector3(9.7f, 0f, -9.3f);
            this.locations[46].gridPoint = new Vector3(10.2f, 0f, -6.5f);
            this.locations[47].gridPoint = new Vector3(9f, 0f, -4.5f);
            this.locations[48].gridPoint = new Vector3(7.5f, 0f, -4f);
            this.locations[49].gridPoint = new Vector3(7.263745f, 0f, -7.441857f);
        }
    }
}