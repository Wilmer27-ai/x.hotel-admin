﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x.hotel
{
    using System;
    using System.Collections.Generic;
        public class occupancyDetails
        {
            public string endDate { get; set; }
            public bool isOccupied { get; set; }
            public string startDate { get; set; }
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
        }

        public class Room
        {
        public string Key { get; set; } // Add this line to include the room key
        public int bedCount { get; set; }

            public occupancyDetails occupancyDetails { get; set; }
            public int roomCapacity { get; set; }
            public string roomClassification { get; set; }
            public int roomDailyRate { get; set; }
            public string roomDescription { get; set; }
            public RoomFeatures roomFeatures { get; set; }
            public int? roomHourlyRate { get; set; } // Nullable int if the property is optional
            public RoomImages roomImages { get; set; }
            public string roomName { get; set; }
            public int roomNumber { get; set; }
        public bool IsOccupied { get; internal set; }
    }

        public class Transaction
        {
            public string customerName { get; set; }
            public string customerPhoneNumber { get; set; }
            public int guestCount { get; set; }
            public string paymentMethod { get; set; }
            public RoomDetails roomDetails { get; set; }
            public int transAmount { get; set; }
            public DateTime transDate { get; set; }
            public string transId { get; set; }
        }

        public class RoomDetails
        {
            public string endDate { get; set; }
            public int roomNumber { get; set; }
            public string startDate { get; set; }
        }
    }
