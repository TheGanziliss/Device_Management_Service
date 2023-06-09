USE [DeviceManagement]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 4/17/2023 3:31:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceName] [varchar](50) NULL,
	[RS232Port] [varchar](50) NULL,
	[Baudrate] [varchar](50) NULL,
	[DeviceID] [varchar](50) NULL,
	[SSID] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[MqttServer] [varchar](50) NULL,
	[Databit] [bit] NULL,
	[IP] [varchar](50) NULL,
	[PORT] [varchar](50) NULL,
	[RoomID] [int] NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 4/17/2023 3:31:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[RoomCondition] [varchar](50) NULL,
	[Refill] [bit] NULL,
 CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
