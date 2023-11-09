using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x.hotel
{
    public class Room
    {

        public int bedCount { get; set; }
        public OccupancyDetails occupancyDetails { get; set; }
        public int roomCapacity { get; set; }
        public string roomClassification { get; set; }
        public int roomDailyRate { get; set; }
        public string roomDescription { get; set; }
        public RoomFeatures roomFeatures { get; set; }
        public RoomImages roomImages { get; set; }
        public string roomName { get; set; }
        public int roomNumber { get; set; }
        public int? roomHourlyRate { get; set; }
        
    }

    public class OccupancyDetails
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool isOccupied { get; set; }
        public string transId { get; set; }
    }

    public class RoomFeatures
    {
        public bool hasCityView { get; set; }
        public bool hasPrivatePool { get; set; }
        public bool hasShower { get; set; }
        public bool isPetFriendly { get; set; }
    }

    public class RoomImages
    {
        public string img1 { get; set; }
        public string img2 { get; set; }
        public string img3 { get; set; }
        public string roomName { get; set; }
    }
}
