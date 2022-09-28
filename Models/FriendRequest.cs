using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        //[ForeignKey("User")]
        public int? SenderID { get; set; }
        //[ForeignKey("User")]
        public int RecieverID { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Reciever { get; set; }
        public FriendRequest()
        {

        }
        public FriendRequest(int? senderID, int recieverID)
        {
            SenderID = senderID;
            RecieverID = recieverID;
        }
    }
}