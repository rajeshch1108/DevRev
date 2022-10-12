using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollabRL : ICollabRL
    {
        private readonly FundooContext fundooContext;
        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public CollabEntity AddCollab(long userId, long noteId, string receiver_email)
        {
            try
            {
                CollabEntity collabEntity = new CollabEntity();
                var result1 = fundooContext.NoteTable.Where(u => u.userID == userId && u.NoteId == noteId).FirstOrDefault();
                var result2 = fundooContext.userTable.Where(u => u.Email == receiver_email).FirstOrDefault();

                if (result1 != null && result2 != null)
                {
                    collabEntity.Sender_UserId = userId;
                    collabEntity.NoteId = noteId;
                    collabEntity.Receiver_Email = receiver_email;
                    collabEntity.Receiver_UserId = result2.userId;
                    fundooContext.CollabTable.Add(collabEntity);
                    fundooContext.SaveChanges();
                    return collabEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
