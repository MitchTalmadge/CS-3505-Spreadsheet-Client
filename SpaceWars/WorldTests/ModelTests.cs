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
            Ship ship1 = new Ship(1, "noob1");
            Ship ship2 = new Ship(2, "noob2");      
            Assert.IsTrue(ship2.Id == 2);
            Assert.IsTrue(ship1.Id == 1);

            Ship shipNeg = new Ship(-8, "lol wtf");
            Assert.IsTrue(shipNeg.Id == -8);
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
        public void TestComponentEquality()
        {
            Star star1 = new Star(new Vector2D(3, 3), 3.0);
            Star star2 = new Star(new Vector2D(3, 3), 3.0);
            Assert.IsFalse(star2.Equals(star1));

            //differentiating between 2 objects with same IDs
            Ship ship1 = new Ship(4, "yo");
            Ship ship2 = new Ship(4, "hmm");
            Assert.IsFalse(ship1.Equals(ship2));
        }

        [TestMethod]
        public void TestAddShips()
        {
            var world = new World(420);
            Ship ship1 = new Ship(1, "noob1");
            Ship ship2 = new Ship(2, "noob2");

            world.PutComponent(new Ship(3, "noob3"));
            Assert.AreEqual(1, world.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Projectile>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Star>().ToList().Count);

            world.PutComponent(ship1);
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);
            //checks that adding a component already in the world doesn not change anything
            world.PutComponent(ship1);
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);
        }

        [TestMethod]
        public void TestRemove()
        {
            var world = new World(420);
            Ship ship1 = new Ship(1, "noob1");
            Ship ship2 = new Ship(2, "noob2");

            world.PutComponent(new Ship(3, "noob3"));
            world.PutComponent(ship1);
            world.PutComponent(ship2);
            Assert.AreEqual(3, world.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Projectile>().ToList().Count);
            Assert.AreEqual(0, world.GetComponents<Star>().ToList().Count);

            world.RemoveComponent(ship1);
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);

            //checks that removing a component not in the world does not change anything
            world.RemoveComponent(new Ship(4, "what"));
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);
        }

        [TestMethod]
        public void TestGetComponent()
        {
            var world = new World(420);
            Ship ship1 = new Ship(1, "noob1");
            Ship ship2 = new Ship(2, "noob2");
            
            world.PutComponent(ship1);
            world.PutComponent(ship2);
            Assert.AreEqual(2, world.GetComponents<Ship>().ToList().Count);
            Assert.AreEqual(ship1, world.GetComponent<Ship>(1));

            //returns null when the component being gotten doesn't exist in the world
            Assert.IsNull(world.GetComponent<Projectile>(1));
        }

        [TestMethod]
        public void TestGetWorldSize()
        {
            var world = new World(420);
            Assert.AreEqual(420, world.Size);
        }
    }
}
