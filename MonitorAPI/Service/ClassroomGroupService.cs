using MonitorAPI.Dao;
using MonitorAPI.Dao.framework;
using MonitorAPI.Model;
using MonitorAPI.Util;
using System;
using System.Collections.Generic;

namespace MonitorAPI.Service
{
    public sealed class ClassroomGroupService
    {
        public void CheckSchedule(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }

        public List<ClassroomGroup> GetClassroomGroups(string sessionID)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomGroupDao classroomGroupDao = new ClassroomGroupDao(pc);
                List<ClassroomGroup> list = classroomGroupDao.GetClassroomGroups();
                return list;
            }
        }

        public ClassroomGroup GetClassroomGroupbyClassroomID(int classroomID)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomGroupDao classroomGroupDao = new ClassroomGroupDao(pc);
                ClassroomGroup classroomGroup = classroomGroupDao.GetClassroomGroupbyClassroomID(classroomID);
                return classroomGroup;
            }
        }

        internal List<ClassroomGroup> GetClassroomGroupListByRoleID(int roleID)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomGroupDao classroomGroupDao = new ClassroomGroupDao(pc);
                List<ClassroomGroup> list = classroomGroupDao.GetClassroomGroupListByRoleID(roleID);
                return list;
            }
        }

        public List<ClassroomGroup> GetClassroomListByGroupID(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }

        public void GroupSchedule(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }
    }
}