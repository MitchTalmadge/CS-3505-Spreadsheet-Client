using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWars
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestAddShips()
        {
            var world = new World(420);
            Ship ship1 = new Ship(1, "noob1");
            Ship ship2 = new Ship(2, "noob2");

            world.UpdateComponent(new Ship(3, "noob3"));
            Assert.AreEqual(1, world.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Projectile>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Star>().ToList().Count);

            world.UpdateComponent(ship1);
        }

        [TestMethod]
        public void TestProjectiles()
        {

        }
    }
}
