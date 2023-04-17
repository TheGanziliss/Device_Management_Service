using System;
using System.Collections.Generic;

namespace DeviceManagementService.Models;

public partial class Device
{
    public int Id { get; set; }

    public string? DeviceName { get; set; }

    public string? Rs232port { get; set; }

    public string? Baudrate { get; set; }

    public string? DeviceId { get; set; }

    public string? Ssid { get; set; }

    public string? Password { get; set; }

    public string? MqttServer { get; set; }

    public bool? Databit { get; set; }

    public string? Ip { get; set; }

    public string? Port { get; set; }

    public int? RoomId { get; set; }
}
