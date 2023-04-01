using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttitudeTests
{
    [TestClass]
    public class TraversingTests
    {
        [TestMethod]
        public void CanTraverse()
        {
            var traverse = Traverse<ShipboardLocations>
                .ForMap(ShipMap.Definition())
                .StartingAt(ShipboardLocations.Forward);
            Assert.IsNotNull(traverse);
            Assert.IsTrue(traverse.Destinations.Count().Equals(2));

        }
    }

    public static class ShipMap
    {
        private static Map<ShipboardLocations> map;

        public static Map<ShipboardLocations> Map => Definition();

        public static Map<ShipboardLocations> Definition()
        {
            map = map ?? MapConstruction();
            return map;
        }

        private static Map<ShipboardLocations> MapConstruction()
        {
            var map = Map<ShipboardLocations>.Construct()

                .PathOf(ShipboardLocations.Aft
                    .LeadsTo(ShipboardLocations.Port))
                .PathOf(ShipboardLocations.Aft
                    .LeadsTo(ShipboardLocations.Starboard))

                .PathOf(ShipboardLocations.Port
                    .LeadsTo(ShipboardLocations.Forward))
                .PathOf(ShipboardLocations.Port
                    .LeadsTo(ShipboardLocations.Aft))
                .PathOf(ShipboardLocations.Port
                    .LeadsTo(ShipboardLocations.Starboard))

                .PathOf(ShipboardLocations.Starboard
                    .LeadsTo(ShipboardLocations.Forward))
                .PathOf(ShipboardLocations.Starboard
                    .LeadsTo(ShipboardLocations.Aft))
                .PathOf(ShipboardLocations.Starboard
                    .LeadsTo(ShipboardLocations.Port))

                .PathOf(ShipboardLocations.Forward
                    .LeadsTo(ShipboardLocations.Port))
                .PathOf(ShipboardLocations.Forward
                    .LeadsTo(ShipboardLocations.Starboard));

            return map;
        }
    }

    public enum ShipboardLocations
    {
        Forward,
        Port,
        Starboard,
        Aft
    }
}