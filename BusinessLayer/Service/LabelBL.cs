using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }

        public LabelEntity CreateLabel(long userId, long noteId, string LabelName)
        {
            try
            {
                return labelRL.CreateLabel(userId, noteId, LabelName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public LabelEntity UpdateLabel(long userId, long noteId, long LabelId, string LabelName)
        {
            try
            {
                return labelRL.UpdateLabel(userId, noteId, LabelId, LabelName);
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
                return labelRL.GetLabel(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    
    }
}
