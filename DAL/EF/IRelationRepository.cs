﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entity;

namespace EF
{
    public interface IRelationRepository
    {
        IQueryable<Relation> All { get; }
        IQueryable<RelationType> RelationTypes { get; }
        IQueryable<RelationType> Tracking { get; }
        void AddRelationType(RelationType relationType);
        void AddRelation(Relation relation);
    }
}
