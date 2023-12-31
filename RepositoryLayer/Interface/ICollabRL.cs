﻿using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollabRL
    {
        public CollabEntity AddCollab(long userId, long noteId, string receiver_email);
        public List<CollabEntity> GetCollab(long userId);
        public bool DeleteCollab(long collabId);
    }
}
