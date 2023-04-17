using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementService.Models;

public partial class DeviceManagementContext : DbContext
{
    public DeviceManagementContext()
    {
    }

    public DeviceManagementContext(DbContextOptions<DeviceManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=" + ServerSettings.ServerIp + ";Database=" + ServerSettings.database +";user id=" + ServerSettings.userName + ";password=" + ServerSettings.password + ";Encrypt=true;TrustServerCertificate=yes");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.Baudrate)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeviceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DeviceID");
            entity.Property(e => e.DeviceName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IP");
            entity.Property(e => e.MqttServer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Port)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PORT");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Rs232port)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RS232Port");
            entity.Property(e => e.Ssid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SSID");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoomCondition)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
