namespace Dijkstra.Data
{
    public static class StaticText
    {
        public static string GameOverSceneName => "GameOverScene";
        public static string PlayerPrefGameOverSign => "GameOverSign";
        public static string TimeOverMessage => "너무 늦어버렸습니다!";
        public static string WrongWayMessage => "You Took a Wrong Way!";
        public static string TooSlowMessage => "You Were Too Slow!";
        public static string StageMessage(int stageNumber) => $"스테이지 {stageNumber}";
        public static string GoalNodeMessage(int targetNode) => $": {targetNode}";
        public static string TimerMessage(float t) => $"Timer: {t:F2}";
    }
    
    [System.Serializable]
    public struct Edge
    {
        public NodeEnum From;
        public NodeEnum To;
        public int Weight;

        public Edge(NodeEnum from, NodeEnum to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public enum NodeEnum
    {
        Node0,
        Node1,
        Node2,
        Node3,
        Node4,
        Node5,
        Node6,
        Node7,
        Node8
    }
    
    public enum GameFail
    {
        TimeOver,
        WrongWay,
        TooSlow
    }
}