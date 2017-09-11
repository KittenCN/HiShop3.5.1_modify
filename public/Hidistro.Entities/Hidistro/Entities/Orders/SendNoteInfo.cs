namespace Hidistro.Entities.Orders
{
    using System;
    using System.Runtime.CompilerServices;

    public class SendNoteInfo
    {
        public string NoteId { get; set; }

        public string Operator { get; set; }

        public string OrderId { get; set; }

        public string Remark { get; set; }
    }
}

