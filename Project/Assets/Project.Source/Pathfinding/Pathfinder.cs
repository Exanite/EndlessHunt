using System;
using System.Collections.Generic;
using Project.Source.Utilities.Components;
using UnityEngine;

namespace Project.Source.Pathfinding
{
    // Aka PathfindingService
    public class Pathfinder : SingletonBehaviour<Pathfinder>
    {
        public List<PathfindingGrid> grids;

        public PathSolver GetSolver(Vector3 position)
        {
            foreach (var grid in grids)
            {
                if (grid.WorldPositionToNode(position) != null)
                {
                    return new PathSolver(grid, Heuristics.Default, new[]
                    {
                        new StraightPathProcessor(),
                    });
                }
            }

            throw new ArgumentException("" +
                $"Couldn't create a {nameof(PathSolver)} for position {position}. " +
                $"There likely is not a {nameof(PathfindingGrid)} at that position");
        }
    }
}