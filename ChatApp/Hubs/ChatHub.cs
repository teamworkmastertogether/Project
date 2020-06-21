using ChatApp.Models.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    /// <summary>
    /// Class xử lý realtime khi thực hiện chat giữa các thành viên
    /// </summary>
    public partial class ChatHub : Hub
    {
        private ChatDbcontext db = new ChatDbcontext();
        public static Dictionary<string, string> LstAllConnections = new Dictionary<string, string>();

        /// <summary>
        /// Hàm xử lý kết nối Hub
        /// Created By: NBDuong 29.04.2020
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            LstAllConnections.Add(Context.ConnectionId, "");
            return base.OnConnected();
        }

        /// <summary>
        /// Hàm xử lý hủy kết nối Hub
        /// Created By: NBDuong 29.04.2020
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            LstAllConnections.Remove(Context.ConnectionId);
            Clients.All.SetStatus(LstAllConnections);
            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Hàm xử lý kết nối từ Hub tới người dùng
        /// Created By: NBDuong 29.04.2020
        /// </summary>
        /// <returns></returns>
        public void SetConnectionUser(string MyUserName)
        {
            LstAllConnections[Context.ConnectionId] = MyUserName;
            Clients.All.SetStatus(LstAllConnections);
        }

        /// <summary>
        /// Hàm dùng để lưu những tin nhắn đã được xem trong khi chat giữa 2 thành viên đang sử dụng hệ thống
        /// Yêu cầu 2 thành viên phải kết bạn mới có quyền chat với nhau
        /// Chức năng chat được thực hiện realtime
        /// Created By: NBDuong 02.05.2020
        /// </summary>
        /// <param name="partnerUserName"></param>
        /// <param name="myUserName"></param>
        public void SaveSeenMessage(string partnerUserName, string myUserName)
        {
            MemberOfListFriend mem = db.Users.FirstOrDefault(s => s.UserName.Equals(myUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(partnerUserName));
            mem.SeenMessage = true;
            db.SaveChanges();
            MemberOfListFriend mem2 = db.Users.FirstOrDefault(s => s.UserName.Equals(partnerUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(myUserName));
            bool checkseen = mem2.SeenMessage;
            List<string> listId = GetListConnectIdByUserName(myUserName, partnerUserName);
            Clients.Clients(listId).SetUserSeen(myUserName, partnerUserName, checkseen);
        }

        /// <summary>
        /// Hàm xử lý check đã xem trong quá trình chat của các thành viên
        /// Created By: NBDuong 03.05.2020
        /// </summary>
        /// <param name="MyUserName"></param>
        /// <param name="PartnerUserName"></param>
        public void AppendSeenForChatBox(string MyUserName, string PartnerUserName)
        {
            List<string> listId = GetListConnectIdByUserName("no value", PartnerUserName);
            Clients.Clients(listId).AppendSeen(MyUserName);
        }

        /// <summary>
        /// Hàm lưu lại những tin nhắn chưa được xem
        /// Created By: NBDuong 03.05.2020
        /// </summary>
        /// <param name="partnerUserName"></param>
        /// <param name="MyUserName"></param>
        public void SaveNotSeenMessage(string partnerUserName, string MyUserName)
        {
            MemberOfListFriend mem = db.Users.FirstOrDefault(s => s.UserName.Equals(partnerUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(MyUserName));
            mem.SeenMessage = false;
            db.SaveChanges();
            List<string> listId = GetListConnectIdByUserName("No value", partnerUserName);
            Clients.Clients(listId).SetNotSeenMessage(MyUserName);
        }

        /// <summary>
        /// Hàm xử lý gửi tin nhắn
        /// Created By: NBDuong 02.05.2020
        /// </summary>
        /// <param name="partnerUserName"></param>
        /// <param name="myUserName"></param>
        /// <param name="currentTime"></param>
        /// <param name="message"></param>
        public void Send(string partnerUserName, string myUserName, string currentTime, string message)
        {
            string Name = db.Users.FirstOrDefault(s => s.UserName.Equals(myUserName)).Name;
            List<string> listId = GetListConnectIdByUserName(myUserName, partnerUserName);
            SaveMessage(partnerUserName, myUserName, currentTime, message);
            SaveNotSeenMessage(partnerUserName, myUserName);
            Clients.Clients(listId).ShowChatBox(Name, myUserName, partnerUserName, message, currentTime);
        }

        /// <summary>
        /// Hàm xử lý lấy ra danh sách cho phép kết nối (cho phép chat) theo username của 2 người
        /// Username của 2 người phải nằm trong danh sách kết nối từ hàm Connected thì mới chat được với nhau
        /// Created By: NBDuong 02.05.2020
        /// </summary>
        /// <param name="MyUserName"></param>
        /// <param name="PartnerUserName"></param>
        /// <returns></returns>
        private List<string> GetListConnectIdByUserName(string MyUserName, string PartnerUserName)
        {
            List<string> listId = new List<string>();
            foreach (KeyValuePair<string, string> entry in LstAllConnections)
            {
                // do something with entry.Value or entry.Key
                if (entry.Value == PartnerUserName || entry.Value == MyUserName)
                {
                    listId.Add(entry.Key);
                }
            }
            return listId;
        }

        /// <summary>
        /// Lưu lại thông tin tin nhắn
        /// Bao gồm các thông tin người gửi, người nhận, thời gian gửi, nội dung tin nhắn, ...
        /// Created By: NBDuong 02.05.2020
        /// </summary>
        /// <param name="PartnerUserName"></param>
        /// <param name="MyUserName"></param>
        /// <param name="currentTime"></param>
        /// <param name="message"></param>
        public void SaveMessage(string PartnerUserName, string MyUserName, string currentTime, string message)
        {
            User user = db.Users.FirstOrDefault(s => s.UserName.Equals(MyUserName));
            Contact contact = db.Contacts
            .FirstOrDefault(s => (s.FromUser.UserName.Equals(MyUserName) && s.ToUser.UserName.Equals(PartnerUserName)) ||
            (s.FromUser.UserName.Equals(PartnerUserName) && s.ToUser.UserName.Equals(MyUserName)));
            Message mess = new Message();
            mess.ContactId = contact.Id;
            mess.UserId = user.Id;
            mess.MessageSend = message;
            mess.TimeSend = currentTime;
            db.Messages.Add(mess);
            SetLastTimeChat(MyUserName, PartnerUserName);
        }

        /// <summary>
        /// Lấy ra thời gian gần nhất gửi tin nhắn
        /// Created By: NBDuong 04.05.2020
        /// </summary>
        /// <param name="MyUserName"></param>
        /// <param name="PartnerUserName"></param>
        public void SetLastTimeChat(string MyUserName, string PartnerUserName)
        {
            MemberOfListFriend mem1 = db.Users.FirstOrDefault(s => s.UserName.Equals(MyUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(PartnerUserName));
            MemberOfListFriend mem2 = db.Users.FirstOrDefault(s => s.UserName.Equals(PartnerUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(MyUserName));
            mem1.TimeLastChat = DateTime.Now;
            mem2.TimeLastChat = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// Lấy user theo username
        /// Created By: NBDuong 01.05.2020
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        private User GetUserByUserName(string UserName)
        {
            return db.Users.FirstOrDefault(s => s.UserName.Equals(UserName));
        }

        /// <summary>
        /// Gửi request check trạng thái tin nhắn đã xem
        /// Created By: NBDuong 06.05.2020
        /// </summary>
        /// <param name="suggestUser"></param>
        /// <param name="MyUserName"></param>
        /// <param name="CheckSend"></param>
        public void SendRequest(string suggestUser, string MyUserName,bool CheckSend)
        {
            ListFriend list = db.ListFriends.FirstOrDefault(s => s.User.UserName.Equals(suggestUser));
            User user = db.Users.FirstOrDefault(s => s.UserName.Equals(MyUserName));
            List<string> listId = GetListConnectIdByUserName(MyUserName, "No value");
            if (CheckSend)
            {
                MemberOfListFriend mem = new MemberOfListFriend();
                mem.AccessRequest = false;
                mem.TimeLastChat = DateTime.Now;
                mem.ListFriendId = list.Id;
                mem.UserId = user.Id;
                mem.SeenMessage = false;
                db.MemberOfListFriends.Add(mem);
                db.SaveChanges();
                Clients.Clients(listId).ChangeStatusSendRequest(suggestUser,1);
            }
            else
            {
                List<MemberOfListFriend> listmem = db.MemberOfListFriends.Where(s => s.ListFriendId == list.Id && s.UserId == user.Id).ToList();
                db.MemberOfListFriends.RemoveRange(listmem);
                db.SaveChanges();
                Clients.Clients(listId).ChangeStatusSendRequest(suggestUser,0);
            }
        }




    }
}