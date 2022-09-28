using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Friends
    {
        public int Id { get; set; }
        //[ForeignKey("User")]
        public int SenderFriendID1 { get; set; }
        //[ForeignKey("User")]
        public int RecieverFriendID2 { get; set; }

        public virtual User SenderFriend1 { get; set; }
        public virtual User RecieverFriend2 { get; set; }
        public Friends()
        {

        }
        public Friends(int senderFriendID1, int recieverFriendID2)
        {
            SenderFriendID1 = senderFriendID1;
            RecieverFriendID2 = recieverFriendID2;
        }
    }
}