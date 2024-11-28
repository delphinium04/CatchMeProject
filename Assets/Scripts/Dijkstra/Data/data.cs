namespace Dijkstra.Data
{
    public static class StaticText
    {
        public static string DijkstraGameOverSceneName => "DijkstraGameOver";
        public static string DijkstraGameSceneName => "Dijkstra";
        public static string PlayerPrefGameOverSign => "GameOverSign";
        public static string PlayerPrefGameOverStage => "GameOverStage";
        public static string TimeOverMessage => "시간이 너무 오래 지났습니다\n경찰에 잡혀버렸습니다!";
        public static string WrongWayMessage => "여기가.. 어디지?\n길을 잃었습니다!";
        public static string TooSlowMessage => "도주 경로가 너무 길어서\n경찰에 잡혔습니다!";
        // public static string StageMessage(int stageNumber) => $"스테이지 {stageNumber}";
        // public static string GoalNodeMessage(int targetNode) => $": {targetNode}";
        // public static string TimerMessage(float t) => $"Timer: {t:F2}";
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
        Node0=0,
        Node1=1,
        Node2=2,
        Node3=3,
        Node4=4,
        Node5=5,
        Node6=6,
        Node7=7,
        Node8=8
    }
    
    public enum GameFail
    {
        TimeOver,
        WrongWay,
        TooSlow
    }
}