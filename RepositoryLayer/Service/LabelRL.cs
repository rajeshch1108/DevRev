using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext fundooContext;

        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public LabelEntity CreateLabel(long userId, long noteId, string LabelName)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();
                var result = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                if (result != null)
                {
                    labelEntity.LabelName = LabelName;
                    labelEntity.NoteId = noteId;
                    labelEntity.UserID = userId;
                    fundooContext.LabelTable.Add(labelEntity);
                    fundooContext.SaveChanges();
                    return labelEntity;

                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public LabelEntity UpdateLabel(long userId, long noteId, long labelID, string LabelName)
        {
            try
            {
                var result1 = fundooContext.NoteTable.Where(note => note.userID == userId && note.NoteId == noteId).FirstOrDefault();
                var result2 = fundooContext.LabelTable.Where(u => u.LabelId == labelID && u.UserID == userId && u.NoteId == noteId).First();
                if (result1 != null && result2 != null)
                {
                    result2.LabelName = LabelName;
                    fundooContext.LabelTable.Update(result2);
                    fundooContext.SaveChanges();
                    return result2;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<LabelEntity> GetLabel(long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(u => u.UserID == userId).ToList();
                if (result != null)
                {
                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteLabel(string labelName)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(u => u.LabelName == labelName).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.LabelTable.Remove(result);
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
