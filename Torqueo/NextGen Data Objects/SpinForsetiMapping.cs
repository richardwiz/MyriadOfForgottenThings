using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torqueo
{
    /// <summary>
    /// USE [NextGen]
    /// CREATE TABLE[dbo].[SpinForsetiMapping](
	/// [Id]
    /// [int] IDENTITY(1,1) NOT NULL,
    /// [Type] [nvarchar](50) NULL,
	/// [ForsetiId] [int] NULL,
	/// [ForsetiName] [nvarchar](100) NULL,
	/// [ForsetiNameShort] [nvarchar](100) NULL,
	/// [SPINId] [nvarchar](50) NULL,
	/// [SPINName] [nvarchar](500) NULL,
	/// [SPINUniqueIdentifierTag] [nvarchar](50) NULL,
	/// [FixtureId] [nvarchar](50) NULL,
	/// [MainEventId] [int] NULL,
	/// [EventFinalised]  [bit] DEFAULT((0)),
	/// [DateFinalised] [datetime] NULL,
	/// [Result]  [nvarchar](5) NULL,
	/// [SubEventId]  [int]  NULL,
	/// [BetTypeId]  [int] NULL,
	/// [OfferExample] [nvarchar](250) NULL,
    /// [StopTransmission] [bit] NOT NULL,
    /// [IsLive] [bit] NOT NULL DEFAULT((0)),
	/// [StopResulting] [bit]  NOT NULL,
    /// [SportId] [int] NULL,
	/// [OffsetInMinutes] [int]  NULL DEFAULT((0)),
	/// [CloseOnPending] [bit]  NOT NULL DEFAULT((1)),
	/// [LeagueId] [int] NULL,
	/// [MeetingId] [int] NULL,
	/// [RequestSnapshot] [bit] NULL,
	/// [MeetingPrefix] [nvarchar](100) NULL,
	/// [Sequence] [int]  NOT NULL DEFAULT((0)),
	/// [DisconnectedFixture] [bit] NOT NULL DEFAULT((0)),
	/// [ImportSetupState] [bit]
    /// </summary>
    public class SpinForsetiMapping
    {
        public virtual Int32 Id { get; protected set;}
        public virtual String Type { get; protected set;}
        public virtual Int32 ForsetiId { get; protected set;}
        public virtual String ForsetiName{ get; protected set;}
        public virtual String ForsetiNameShort { get; protected set;}
        public virtual String SPINId { get; protected set;}
        public virtual String SPINName { get; protected set;}
        public virtual String SPINUniqueIdentifierTag { get; protected set;}
        public virtual String FixtureId { get; protected set;}
        public virtual Int32 MainEventId { get; protected set;}
        public virtual Boolean EventFinalised { get; protected set;}
        public virtual DateTime? DateFinalised { get; protected set;}
        public virtual String Result { get; protected set;}
        public virtual Int32 SubEventId { get; protected set;}
        public virtual Int32 BetTypeId { get; protected set;}
        public virtual String OfferExample { get; protected set;}
        public virtual Boolean StopTransmission { get; protected set;}
        public virtual Boolean IsLive { get; protected set;}
        public virtual Boolean StopResulting { get; protected set;}
        public virtual Int32 SportId { get; protected set;}
        public virtual Int32 OffsetInMinutes { get; protected set;}
        public virtual Boolean CloseOnPending { get; protected set;}
        public virtual Int32 LeagueId { get; protected set;}
        public virtual Int32 MeetingId { get; protected set;}
        public virtual Boolean RequestSnapshot { get; protected set;}
        public virtual String MeetingPrefix { get; protected set;}
        public virtual Int32 Sequence { get; protected set;}
        public virtual Boolean DisconnectedFixture { get; protected set;}
        public virtual Boolean ImportSetupState { get; protected set;}

        public SpinForsetiMapping()
        {
           this.Id = 0;
           this.Type = String.Empty;
           this.ForsetiId = 0;
           this.ForsetiName = String.Empty;
           this.ForsetiNameShort = String.Empty; 
           this.SPINId = String.Empty;
           this.SPINName = String.Empty;
           this.SPINUniqueIdentifierTag = String.Empty;
           this.FixtureId = String.Empty;
           this.MainEventId = 0;
           this.EventFinalised = false;
           this.DateFinalised = null;
           this.Result = String.Empty;
           this.SubEventId = 0;
           this.BetTypeId = 0;
           this.OfferExample = String.Empty;
           this.StopTransmission = false;
           this.IsLive = false;
           this.StopResulting = false;
           this.SportId = 0;
           this.OffsetInMinutes = 0;
           this.CloseOnPending = false;
           this.LeagueId = 0;
           this.MeetingId = 0;
           this.RequestSnapshot = false;
           this.MeetingPrefix = String.Empty;
           this.Sequence = 0;
           this.DisconnectedFixture = false;
           this.ImportSetupState = false;
        }
    }
}
