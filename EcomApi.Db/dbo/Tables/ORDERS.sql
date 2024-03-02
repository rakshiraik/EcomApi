﻿CREATE TABLE [dbo].[ORDERS] (
    [ORDERID]          INT           IDENTITY (1, 1) NOT NULL,
    [CUSTOMERID]       NVARCHAR (10) NULL,
    [ORDERDATE]        DATE          NULL,
    [DELIVERYEXPECTED] DATE          NULL,
    [CONTAINSGIFT]     BIT           NULL,
    PRIMARY KEY CLUSTERED ([ORDERID] ASC)
);

