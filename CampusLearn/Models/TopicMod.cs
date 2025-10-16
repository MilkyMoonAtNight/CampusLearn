namespace CampusLearn.Models
{
    public class TopicMod
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; } = "";
        public int? ClusterID { get; set; }
        public long? ModuleHeadID { get; set; }
    }
}
