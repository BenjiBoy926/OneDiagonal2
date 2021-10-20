using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillClimbingLocalSearch<Config> : LocalSearch<Config>
{
    #region Constructors
    public HillClimbingLocalSearch(Func<Config, List<Config>> getNeighbors, 
        Func<Config, int> getValue,
        Func<List<Config>, Config> tieBreaker, 
        Func<Config, bool> goalTest, 
        Func<Config, int, bool> terminalTest)
        : base(getNeighbors, getValue, tieBreaker, goalTest, terminalTest) { }
    #endregion

    #region Public Methods
    public override bool Search(Config startingConfiguration, out Config resultingConfiguration)
    {
        resultingConfiguration = startingConfiguration;
        int currentValue = getValue(resultingConfiguration);
        int currentIteration = 0;

        // Loop while the resume condition returns true
        while(!goalTest.Invoke(resultingConfiguration) && !terminalTest(resultingConfiguration, currentIteration))
        {
            // Get the neighbors of the current configuration
            List<Config> neighbors = getNeighbors.Invoke(resultingConfiguration);
            // This list stores all neighbors with the highest value
            List<Config> neighborsWithHighestValue = new List<Config>();

            foreach(Config neighbor in neighbors)
            {
                int value = getValue(neighbor);

                // If the value of the current neighbor is greater than the current value,
                // then clear the list of lowest value neighbors and add this as the first one
                if(value > currentValue)
                {
                    currentValue = value;
                    neighborsWithHighestValue.Clear();
                    neighborsWithHighestValue.Add(neighbor);
                }
                // If the value of the current neighbor is equal to the current value,
                // then add it to the list of neighbors with highest value
                else if(value == currentValue)
                {
                    neighborsWithHighestValue.Add(neighbor);
                }
            }

            // If there are no neighbors with a higher value, we couldn't satisfying the 
            // goal test, so return now with value "false"
            if (neighborsWithHighestValue.Count == 0) return false;
            // If there is only one neighbor with the lowest cost, choose it as the current config
            if (neighborsWithHighestValue.Count == 1) resultingConfiguration = neighborsWithHighestValue[0];
            // If there are multiple neighbors with the same lowest cost,
            // use the tie breaker to determine which gets picked
            else if (neighborsWithHighestValue.Count > 1) resultingConfiguration = tieBreaker.Invoke(neighborsWithHighestValue);

            // Update current iteration before next step
            currentIteration++;
        }

        // After the loop, return true if the resulting configuration is a goal
        return goalTest.Invoke(resultingConfiguration);
    }
    #endregion
}
