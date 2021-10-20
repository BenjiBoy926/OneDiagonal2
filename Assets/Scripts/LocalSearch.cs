using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocalSearch<Config>
{
    #region Protected Fields
    // Function used to get the neighbors of the given configuration
    protected Func<Config, List<Config>> getNeighbors;
    // Get the cost of a given configuration
    protected Func<Config, int> getValue;
    // This function is used to break the tie between multiple configurations with the same cost
    protected Func<List<Config>, Config> tieBreaker;
    // Test the given configuration to see if it is the goal
    protected Func<Config, bool> goalTest;
    // A test to determine if the algorithm should terminate prematurely,
    // without having found a goal
    protected Func<Config, int, bool> terminalTest;
    #endregion

    #region Constructors
    protected LocalSearch(Func<Config, List<Config>> getNeighbors, 
        Func<Config, int> getValue,
        Func<List<Config>, Config> tieBreaker, 
        Func<Config, bool> goalTest, 
        Func<Config, int, bool> terminalTest)
    {
        this.getNeighbors = getNeighbors;
        this.getValue = getValue;
        this.tieBreaker = tieBreaker;
        this.goalTest = goalTest;
        this.terminalTest = terminalTest;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Search a configuration that satisfies the goal test
    /// Return false if no config could be found that satisfies the goal test
    /// </summary>
    /// <param name="startingConfiguration"></param>
    /// <param name="resultingConfiguration"></param>
    /// <returns></returns>
    public abstract bool Search(Config startingConfiguration, out Config resultingConfiguration);
    #endregion
}
