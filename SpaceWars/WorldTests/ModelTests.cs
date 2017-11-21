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
            World w = new World(420, 69);
            w.UpdateComponent(new Ship());
            Assert.AreEqual(1, w.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(0, w.GetComponents<Projectile>().ToList().Count);
            Assert.AreEqual(0, w.GetComponents<Star>().ToList().Count);


        }

        [TestMethod]
        public void TestProjectiles()
        {

        }
    }
}
