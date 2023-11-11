using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x.hotel
{
    using System;
    using System.Collections.Generic;
        public class Admin
        {
            public string AdminId { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class OccupancyDetails
        {
            public string EndDate { get; set; }
            public bool IsOccupied { get; set; }
            public string StartDate { get; set; }
            public string TransId { get; set; }
        }

        public class RoomFeatures
        {
            public bool HasCityView { get; set; }
            public bool HasPrivatePool { get; set; }
            public bool HasShower { get; set; }
            public bool IsPetFriendly { get; set; }
        }

        public class RoomImages
        {
            public string Img1 { get; set; }
            public string Img2 { get; set; }
            public string Img3 { get; set; }
        }

        public class Room
        {
            public int BedCount { get; set; }
            public OccupancyDetails OccupancyDetails { get; set; }
            public int RoomCapacity { get; set; }
            public string RoomClassification { get; set; }
            public int RoomDailyRate { get; set; }
            public string RoomDescription { get; set; }
            public RoomFeatures RoomFeatures { get; set; }
            public int? RoomHourlyRate { get; set; } // Nullable int if the property is optional
            public RoomImages RoomImages { get; set; }
            public string RoomName { get; set; }
            public int RoomNumber { get; set; }
        }

        public class Transaction
        {
            public string CustomerName { get; set; }
            public string CustomerPhoneNumber { get; set; }
            public int GuestCount { get; set; }
            public string PaymentMethod { get; set; }
            public RoomDetails RoomDetails { get; set; }
            public int TransAmount { get; set; }
            public DateTime TransDate { get; set; }
            public string TransId { get; set; }
        }

        public class RoomDetails
        {
            public string EndDate { get; set; }
            public int RoomNumber { get; set; }
            public string StartDate { get; set; }
        }

        public class Root
        {
            public Dictionary<string, Admin> Admins { get; set; }
            public Dictionary<string, Room> Rooms { get; set; }
            public Dictionary<string, Transaction> Transactions { get; set; }
        }
    }
