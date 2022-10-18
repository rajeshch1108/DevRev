using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity CreateLabel(long userId, long noteId, string LabelName);
        public LabelEntity UpdateLabel(long userId, long noteId, long LabelId, string LabelName);
        public List<LabelEntity> GetLabel(long userId);
        public bool DeleteLabel(string labelName);

    }
}
