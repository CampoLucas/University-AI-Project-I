using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Pathfinding
{
    /// <summary>
    /// Lo elegí porque incorpora tanto la distancia hasta la meta como el costo de moverse por el mundo. 
    /// Esto lo hace ideal para terrenos complejos, obstáculos dinámicos y búsqueda de caminos más inteligente y eficiente.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AStar<T>
    {
        public List<T> Run(T start, 
            Func<T, bool> satiesfies, 
            Func<T, List<T>> connections, 
            Func<T, T, float> getCost, 
            Func<T, float> heuristic, 
            int watchdog = 500)
        {
            var pending = new PriorityQueue<T>();
            var visited = new HashSet<T>();
            var parent = new Dictionary<T, T>();
            var cost = new Dictionary<T, float>();

            pending.Enqueue(start, 0);
            cost[start] = 0;

            while (watchdog > 0 && !pending.IsEmpty)
            {
                watchdog--;
                var curr = pending.Dequeue();

                if (satiesfies(curr))
                {
                    return ReconstructPath(parent, curr);
                }

                visited.Add(curr);
                var neighbours = connections(curr);
                for (var i = 0; i < neighbours.Count; i++)
                {
                    var neigh = neighbours[i];
                    if (visited.Contains(neigh)) continue;

                    var tentativeCost = cost[curr] + getCost(curr, neigh);
                    if (cost.ContainsKey(neigh) && cost[neigh] < tentativeCost) continue;

                    parent[neigh] = curr;
                    cost[neigh] = tentativeCost;

                    var priority = tentativeCost + heuristic(neigh);
                    pending.Enqueue(neigh, priority);
                }

                if (watchdog <= 0 || pending.IsEmpty)
                {
                    // No path found
                    Logging.LogError("Watchdog called");
                    return new List<T>();
                }
            }
            Logging.LogError("Watchdog called");
            return new List<T>();
        }

        public List<T> CleanPath(List<T> path, Func<T, T, bool> inView)
        {
            if (path == null) return path;
            if (path.Count <= 2) return path;

            var list = new List<T>
            {
                path[0]
            };
            for (var i = 2; i < path.Count - 2; i++)
            {
                var gp = list[list.Count - 1];
                if (!inView(gp, path[i]))
                {
                    list.Add(path[i - 1]);
                }
            }
            list.Add(path[path.Count - 1]);


            return list;
        }

        private List<T> ReconstructPath(Dictionary<T, T> parent, T goal)
        {
            var path = new List<T>();
            path.Add(goal);
            while (parent.ContainsKey(path[path.Count - 1]))
            {
                var father = parent[path[path.Count - 1]];
                path.Add(father);
            }
            path.Reverse();
            return path;
        }

        public void CleanPath(List<T> path, Func<T, T, bool> inView, out List<T> cleanedPath)
        {
            cleanedPath = new List<T>();
            if (path == null || path.Count <= 2)
            {
                cleanedPath = path;
                return;
            }

            cleanedPath.Add(path[0]);
            for (var i = 2; i < path.Count - 2; i++)
            {
                var gp = cleanedPath[cleanedPath.Count - 1];
                if (!inView(gp, path[i]))
                {
                    cleanedPath.Add(path[i - 1]);
                }
            }
            cleanedPath.Add(path[path.Count - 1]);
            return;
        }
    }

    public class LazyAStar<T>
    {
        public IEnumerable<T> Run(T start,
            Func<T, bool> satiesfies,
            Func<T, List<T>> connections,
            Func<T, T, float> getCost,
            Func<T, float> heuristic,
            int watchdog = 500)
        {
            var pending = new PriorityQueue<T>();
            var visited = new HashSet<T>();
            var parent = new Dictionary<T, T>();
            var cost = new Dictionary<T, float>();

            pending.Enqueue(start, 0);
            cost[start] = 0;

            while (watchdog > 0 && !pending.IsEmpty)
            {
                watchdog--;
                var curr = pending.Dequeue();

                if (satiesfies(curr))
                {
                    ReconstructPath(parent, curr);
                }

                visited.Add(curr);
                var neighbours = connections(curr);
                for (var i = 0; i < neighbours.Count; i++)
                {
                    var neigh = neighbours[i];
                    if (visited.Contains(neigh)) continue;

                    var tentativeCost = cost[curr] + getCost(curr, neigh);
                    if (cost.ContainsKey(neigh) && cost[neigh] < tentativeCost) continue;

                    parent[neigh] = curr;
                    cost[neigh] = tentativeCost;

                    var priority = tentativeCost + heuristic(neigh);
                    pending.Enqueue(neigh, priority);
                }

                if (watchdog <= 0 || pending.IsEmpty)
                {
                    // No path found
                    yield break;
                }
            }
            yield break;
        }

        public List<T> CleanPath(List<T> path, Func<T, T, bool> inView)
        {
            if (path == null) return path;
            if (path.Count <= 2) return path;

            var list = new List<T>
            {
                path[0]
            };
            for (var i = 2; i < path.Count - 2; i++)
            {
                var gp = list[list.Count - 1];
                if (!inView(gp, path[i]))
                {
                    list.Add(path[i - 1]);
                }
            }
            list.Add(path[path.Count - 1]);


            return list;
        }

        private List<T> ReconstructPath(Dictionary<T, T> parent, T goal)
        {
            var path = new List<T>();
            path.Add(goal);
            while (parent.ContainsKey(path[path.Count - 1]))
            {
                var father = parent[path[path.Count - 1]];
                path.Add(father);
            }
            path.Reverse();
            return path;
        }

        public void CleanPath(List<T> path, Func<T, T, bool> inView, out List<T> cleanedPath)
        {
            cleanedPath = new List<T>();
            if (path == null || path.Count <= 2)
            {
                cleanedPath = path;
                return;
            }

            cleanedPath.Add(path[0]);
            for (var i = 2; i < path.Count - 2; i++)
            {
                var gp = cleanedPath[cleanedPath.Count - 1];
                if (!inView(gp, path[i]))
                {
                    cleanedPath.Add(path[i - 1]);
                }
            }
            cleanedPath.Add(path[path.Count - 1]);
            return;
        }
    }
}
