﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.SyncService.SyncEntities;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class MembersSyncRemoteService : ISyncDataService
    {
        public TimeSpan SyncInterval
        {
            get
            {
                return new TimeSpan(0, 30, 0);
            }
        }
        public string Name
        {
            get
            {
                return this.GetType().ToString();
            }

        }
        public IEnumerable<ISyncDataObject> GetSyncObjects(int companyId, string storeId)
        {
            try
            {
                using (var db = SyncDbContextFactory.Factory<EFDbContext>())
                {
                    var result = db.Members.Where(o => o.CompanyId == companyId).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
                    return result;
                }
            }
            catch
            {
                return new List<ISyncDataObject>();
            }
        }

        public ISyncDataObject GetItem(Guid guid, int companyId, string storeId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var entity = db.Members.Where(o => o.SyncItemId == guid && o.CompanyId == companyId).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.Member().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.Members.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.Member().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            var temp = data as Member;
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                Pharos.Logic.Entity.Members entity;
                if (!db.Members.Any(o => o.SyncItemId == guid))
                {
                    entity = new Pharos.Logic.Entity.Members();
                    entity.InitEntity(temp, false);
                    db.Members.Add(entity);
                    db.SaveChanges();
                }
                else
                {
                    entity = db.Members.FirstOrDefault(o => o.SyncItemId == guid);
                }
                return entity.SyncItemVersion;
            }
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            var temp = mergedData as Member;
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var dbEntity = db.Members.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
                if (dbEntity == null)
                    throw new SyncService.Exceptions.SyncException("未能找到更新的项！");
                dbEntity.InitEntity(temp, false);
                db.SaveChanges();
                var newEntity = db.Members.FirstOrDefault(o => o.SyncItemId == guid);
                return newEntity.SyncItemVersion;
            }
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            throw new SyncException("Members表不允许远程删除！");
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            return syncDataObject1;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.UploadAndDownload; }
        }
    }
}
