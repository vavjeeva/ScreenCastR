using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScreenCastApp
{
    public class ScreenCastManager
    {
        private List<Viewer> viewers = new List<Viewer>();

        public void AddViewer(string connectionId, string agentName)
        {
            viewers.Add(new Viewer(connectionId, agentName));
        }

        public void RemoveViewer(string connectionId)
        {
            viewers.Remove(viewers.First(i => i.ConnectionId == connectionId));
        }

        public void RemoveViewerByAgent(string agentName)
        {
            viewers.RemoveAll(i => i.AgentName == agentName);
        }

        public bool IsViewerExists(string agentName)
        {
            return viewers.Any(i => i.AgentName == agentName);
        }

    }

    internal class Viewer
    {
        public string ConnectionId { get; set; }
        public string AgentName { get; set; }

        public Viewer(string connectionId, string agentName)
        {
            ConnectionId = connectionId;
            AgentName = agentName;
        }
    }
}
