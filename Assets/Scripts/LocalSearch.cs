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
    // A test to determine if the algorithm should terminate prematurely,
    // without having found a local max
    protected Func<Config, int, bool> terminalTest;
    #endregion

    #region Constructors
    protected LocalSearch(Func<Config, List<Config>> getNeighbors, 
        Func<Config, int> getValue,
        Func<List<Config>, Config> tieBreaker, 
        Func<Config, int, bool> terminalTest)
    {
        this.getNeighbors = getNeighbors;
        this.getValue = getValue;
        this.tieBreaker = tieBreaker;
        this.terminalTest = terminalTest;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Search for a configuration with the local max value and return it
    /// </summary>
    /// <param name="startingConfiguration"></param>
    /// <returns>True if a local maximum was found and false if none could be found before the terminal test returned true</returns>
    public abstract bool Search(Config startingConfiguration, out Config resultingConfiguration);
    #endregion
}
