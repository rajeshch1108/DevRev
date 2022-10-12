using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class CollabBL : ICollabBL
    {
        private readonly ICollabRL collabRL;

        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }

        public CollabEntity AddCollab(long userId, long noteId, string receiver_email)
        {
            try
            {
                return collabRL.AddCollab(userId, noteId, receiver_email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CollabEntity> GetCollab(long userId)
        {
            try
            {
                return collabRL.GetCollab(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteCollab(long collabId)
        {
           try 
	    {	        
           return collabRL.DeleteCollab(collabId);
		
	    }
	       catch (Exception ex)
	    {

	    	throw ex;
	    }
       }
    }
}
