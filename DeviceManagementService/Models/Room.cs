using System;
using System.Collections.Generic;

namespace DeviceManagementService.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? Name { get; set; }

    public string? RoomCondition { get; set; }

    public bool? Refill { get; set; }
}
