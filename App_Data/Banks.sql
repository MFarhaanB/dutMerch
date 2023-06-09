USE [DefaultConnection]
GO
SET IDENTITY_INSERT [dbo].[BankInfoes] ON 

INSERT [dbo].[BankInfoes] ([Id], [BankName], [BranchCode], [Address], [City], [BankCode]) VALUES (1, N'FNB', N'433892DF7', N'2 arum road bluff', N'durban', N'64536436346345')
INSERT [dbo].[BankInfoes] ([Id], [BankName], [BranchCode], [Address], [City], [BankCode]) VALUES (2, N'Absa', N'35432', N'A 45 Ekurhuleni', N'JHB', N'93928398NHSKS34532')
SET IDENTITY_INSERT [dbo].[BankInfoes] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentMethods] ON 

INSERT [dbo].[PaymentMethods] ([Id], [Name], [Description], [Key]) VALUES (1, N'PayFast', N'PayFast', N'PY_PayFast')
INSERT [dbo].[PaymentMethods] ([Id], [Name], [Description], [Key]) VALUES (2, N'EFT', N'EFT', N'PY_EFT')
INSERT [dbo].[PaymentMethods] ([Id], [Name], [Description], [Key]) VALUES (3, N'COD', N'Cash On Delivery', N'PY_COD')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
	GO
