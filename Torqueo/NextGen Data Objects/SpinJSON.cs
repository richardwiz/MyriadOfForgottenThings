using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torqueo
{
    /// <summary>
    /// Maps to NextGen.dbo.SpinJson, thus;
    /// USE [NextGen]
    /// CREATE TABLE[dbo].[SpinJson](
    /// [Id] [int] IDENTITY(1,1) NOT NULL,
    /// [Sport] [nvarchar](50) NOT NULL,
    /// [Imported] [datetime]
    /// </summary>
    public class SpinJson
    {
        public virtual int JsonId { get; protected set; }
        public virtual String Sport { get; protected set; }
        public virtual DateTime Imported { get; protected set; }

        public SpinJson()
        {
            this.JsonId = 0;
            this.Sport = String.Empty;
            this.Imported = DateTime.Now;
        }

    }
}
