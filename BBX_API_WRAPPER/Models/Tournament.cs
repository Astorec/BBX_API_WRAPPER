using System.ComponentModel.DataAnnotations;

namespace BBX_API_WRAPPER.Models
{
    public class Tournament
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int? participants { get; set; }
        public int is_side_event { get; set; }
        public int region { get; set; }
        public int attendance_id { get; set; }
        public int finalized { get; set; }
        public string state { get; set; }
        public int is_store_championship { get; set; }
    }
}