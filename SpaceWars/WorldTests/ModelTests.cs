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
            //verify ship IDs are the ship count increment
            
            Assert.IsTrue(ship2.Id == ship1.Id + 1);
        }

        [TestMethod]
        public void TestCreateProjectiles()
        {
            Projectile proj3 = new Projectile(1);
            Projectile proj1 = new Projectile(1);
            Assert.IsTrue(proj1.Id == proj3.Id + 1);
        }

        [TestMethod]
        public void TestCreateStars()
        {
            Star star1 = new Star(new Vector2D(3,3), 3.0);
            Star star2 = new Star(new Vector2D(3, 3), 3.0);
            Assert.IsTrue(star2.Id == star1.Id + 1);
        }

        [TestMethod]
        public void TestAddShips()
        {
            var world = new World(420);
            Ship ship1 = new Ship("noob1");
            Ship ship2 = new Ship("noob2");

            world.PutComponent(new Ship("noob3"));
            Assert.AreEqual(1, world.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Projectile>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Star>().ToList().Count);
            world.PutComponent(ship1);
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);
        }

        [TestMethod]
        public void TestProjectiles()
        {

        }
    }
}
