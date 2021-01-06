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
        public List<Location> locations;

        // Refactor this to make it easier to read
        public void SetUpLocations()
        {
            this.locations = new List<Location>();
            this.locations.Add(new Location("GO", new Vector3(-25.0f, 0f, 25.0f)));
        }
    }
}