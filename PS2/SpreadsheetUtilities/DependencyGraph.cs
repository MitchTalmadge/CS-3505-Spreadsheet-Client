// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

/// <summary>
/// Jiahui Chen
/// u0980890
/// PS2
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// This Dictionary maps a string to a HashSet of its dependents,
        /// the strings that depend on it and must be evaluated after.
        /// 
        /// The Keys are the dependees. 
        /// </summary>
        private Dictionary<string, HashSet<string>> dependents;

        /// <summary>
        /// This Dictionary maps a string to a HashSet of the strings it's a dependee of,
        /// the strings that it depends on and must be evaluated before.
        /// 
        /// The keys are the dependents. 
        /// </summary>
        private Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependees = new Dictionary<string, HashSet<string>>();
            dependents = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {
                int total = 0;
                foreach (var dependeesSet in dependees.Values)
                {
                    total += dependeesSet.Count;
                }
                return total;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a"), how many 
        /// 
        /// </summary>
        public int this[string s]
        {
            get { return getDependeesSize(s); }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (dependents.TryGetValue(s, out HashSet<string> set))
            {
                return set.Count > 0;
            }
            return false;
        }
        
        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (getDependeesSize(s) != 0)
            {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependents.TryGetValue(s, out HashSet<string> originalDependents))
            {
                HashSet<string> dependentsCopy = new HashSet<string>(originalDependents);
                return dependentsCopy;
            }
            return new List<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependees.TryGetValue(s, out HashSet<string> originalDependees))
            {
                HashSet<string> dependeesCopy = new HashSet<string>(originalDependees);
                return dependeesCopy;
            }
            return new List<string>();
        }
        
        /// <summary>
        /// <para>Adds the ordered pair (dependee,dependent), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   dependent depends on dependee
        ///
        /// </summary>
        /// <param name="dependee"> dependee must be evaluated first. T depends on S</param>
        /// <param name="dependent"> dependent cannot be evaluated until s is</param>        /// 
        public void AddDependency(string dependee, string dependent)
        {
            //checks for if dependency relationship already exists
            if (dependees.ContainsKey(dependent))
            {
                if (!dependees[dependent].Contains(dependee))
                {
                    dependees[dependent].Add(dependee);
                }
            }
            else //if it doesn't then add relationship
            {
                HashSet<string> newDependees = new HashSet<string>();
                newDependees.Add(dependee);
                dependees.Add(dependent, newDependees);
            }
            if (dependents.ContainsKey(dependee))
            {
                if (!dependents[dependee].Contains(dependent))
                {
                    dependents[dependee].Add(dependent);
                }
            }
            else
            {
                HashSet<string> newDependents = new HashSet<string>();
                newDependents.Add(dependent);
                dependents.Add(dependee, newDependents);
            }
        }

        /// <summary>
        /// Removes the ordered pair (dependee,dependent), if it exists
        /// </summary>
        /// <param name="dependee"></param>
        /// <param name="dependent"></param>
        public void RemoveDependency(string dependee, string dependent)
        {
            if (dependees.TryGetValue(dependent, out HashSet<string> depees))
            {
                depees.Remove(dependee);
            }
            if (dependents.TryGetValue(dependee, out HashSet<string> deps))
            {
                deps.Remove(dependent);
            }
        }
        
        /// <summary>
        /// Removes all existing ordered pairs of the form (dependee,r).  Then, for each
        /// t in newDependents, adds the ordered pair (dependee,t).
        /// </summary>
        public void ReplaceDependents(string dependee, IEnumerable<string> newDependents)
        {
            //if the dependee already has dependents, remove them
            if (dependents.TryGetValue(dependee, out HashSet<string> removed))
            {
                foreach (string removedDep in removed)
                {
                    dependees[removedDep].Remove(dependee);
                }
                removed.Clear();
            }
            HashSet<string> replacementDeps = new HashSet<string>(newDependents);
            foreach (string newDep in replacementDeps)
            {
                this.AddDependency(dependee, newDep);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string dependent, IEnumerable<string> newDependees)
        {
            //if the dependent already has dependees, remove them
            if (dependees.TryGetValue(dependent, out HashSet<string> removed))
            {
                foreach (string removedDep in removed)
                {
                    dependents[removedDep].Remove(dependent);
                }
                removed.Clear();
            }
            HashSet<string> replacementDeps = new HashSet<string>(newDependees);
            foreach(string newDep in replacementDeps)
            {
                this.AddDependency(newDep, dependent);
            }
        }

        /////////////////   H E L P E R   M E T H O D S   ////////////////

        /// <summary>
        /// Gets the number of dependees the parameter string/dependent has.
        /// (Gets number of strings the paramater depends on)
        /// If the param string is not mapped 0 is returned. 
        /// </summary>
        private int getDependeesSize(string dependent)
        {
            if (dependees.TryGetValue(dependent, out HashSet<string> set))
            {
                return set.Count;
            }
            return 0; 
        }
    }
}