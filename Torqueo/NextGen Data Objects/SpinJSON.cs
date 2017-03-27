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
    public class SpinJSON
    {
        public int JsonId { get; set; }
        public String Sport { get; set; }
        public DateTime Imported { get; set; }

        public SpinJSON()
        {
            this.JsonId = 0;
            this.Sport = String.Empty;
            this.Imported = DateTime.Now;
        }
    }
}
