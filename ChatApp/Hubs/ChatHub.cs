using ChatApp.Models.Entities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private ChatDbcontext db = new ChatDbcontext();
        public static Dictionary<string, string> LstAllConnections = new Dictionary<string, string>();

        public override Task OnConnected()
        {
            LstAllConnections.Add(Context.ConnectionId, "");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            LstAllConnections.Remove(Context.ConnectionId);
            Clients.All.SetStatus(LstAllConnections);
            return base.OnDisconnected(stopCalled);
        }

        public void SetConnectionUser(string MyUserName)
        {
            LstAllConnections[Context.ConnectionId] = MyUserName;
            Clients.All.SetStatus(LstAllConnections);
        }

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

        public void AppendSeenForChatBox(string MyUserName, string PartnerUserName)
        {
            List<string> listId = GetListConnectIdByUserName("no value", PartnerUserName);
            Clients.Clients(listId).AppendSeen(MyUserName);
        }

        public void SaveNotSeenMessage(string partnerUserName, string MyUserName)
        {
            MemberOfListFriend mem = db.Users.FirstOrDefault(s => s.UserName.Equals(partnerUserName))
            .ListFriends.First().MemberOfListFriends.FirstOrDefault(s => s.User.UserName.Equals(MyUserName));
            mem.SeenMessage = false;
            db.SaveChanges();
            List<string> listId = GetListConnectIdByUserName("No value", partnerUserName);
            Clients.Clients(listId).SetNotSeenMessage(MyUserName);
        }

        public void Send(string partnerUserName, string myUserName, string currentTime, string message)
        {
            string Name = db.Users.FirstOrDefault(s => s.UserName.Equals(myUserName)).Name;
            List<string> listId = GetListConnectIdByUserName(myUserName, partnerUserName);
            SaveMessage(partnerUserName, myUserName, currentTime, message);
            SaveNotSeenMessage(partnerUserName, myUserName);
            Clients.Clients(listId).ShowChatBox(Name, myUserName, partnerUserName, message, currentTime);
        }

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

        private User GetUserByUserName(string UserName)
        {
            return db.Users.FirstOrDefault(s => s.UserName.Equals(UserName));
        }
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