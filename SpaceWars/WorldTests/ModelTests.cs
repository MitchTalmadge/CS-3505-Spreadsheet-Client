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
        public void TestCreateShips()
        {
            Ship ship1 = new Ship("noob1");
            Ship ship2 = new Ship("noob2");
            Assert.AreEqual(1, ship1.Id);
            Assert.AreEqual(2, ship2.Id);
        }

        [TestMethod]
        public void TestAddShips()
        {
            var world = new World(420);
            Ship ship1 = new Ship("noob1");
            Ship ship2 = new Ship("noob2");

            world.UpdateComponent(new Ship("noob3"));
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
