﻿using Pharos.Logic.ApiData.Pos.DAL;
using Pharos.Logic.DAL;
using Pharos.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pharos.SyncService.Helpers;
using Pharos.SyncService.SyncEntities;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.RemoteDataServices
{
    public class NoticeSyncRemoteService : ISyncDataService
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
                    var result = db.Notices.Where(o => o.CompanyId == companyId && (("," + o.StoreId + ",").Contains("," + storeId + ",") || ("," + o.StoreId + ",").Contains(",-1,"))).Select(o => new SyncDataObject() { SyncItemId = o.SyncItemId, SyncItemVersion = o.SyncItemVersion }).ToList();
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
                var entity = db.Notices.Where(o => o.SyncItemId == guid && o.CompanyId == companyId && (("," + o.StoreId + ",").Contains("," + storeId + ",") || ("," + o.StoreId + ",").Contains(",-1,"))).FirstOrDefault();
                if (entity != null)
                    return new Pharos.SyncService.SyncEntities.Notice().InitEntity(entity);
                return null;
            }
        }
        public IDictionary<Microsoft.Synchronization.SyncId, ISyncDataObject> GetItems(IEnumerable<Microsoft.Synchronization.SyncId> ids, int companyId, string StoreId)
        {
            using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            {
                var syncidsdict = ids.ToDictionary(o => o.GetGuidId(), o => o);
                var syncids = syncidsdict.Keys;
                var datas = db.Notices.Where(o => syncids.Contains(o.SyncItemId)).ToList();
                return datas.ToDictionary(o => syncidsdict[o.SyncItemId], o => (ISyncDataObject)new Pharos.SyncService.SyncEntities.Notice().InitEntity(o));
            }
        }
        public byte[] CreateItem(ISyncDataObject data, Guid guid, int companyId, string storeId)
        {
            //var temp = data as Pharos.SyncService.SyncEntities.Notice;
            //using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            //{
            //    var entity = new Pharos.Logic.Entity.Notice();
            //    entity.InitEntity(temp, false);
            //    db.Notices.Add(entity);
            //    db.SaveChanges();
            //    return entity.SyncItemVersion;
            //}
            // 该表不允许从本地修改
            throw new SyncException("Notice表不允许本地更新");
        }

        public byte[] UpdateItem(Guid guid, ISyncDataObject mergedData, int companyId, string storeId)
        {
            //var temp = mergedData as Pharos.SyncService.SyncEntities.Notice;
            //using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            //{
            //    var dbEntity = db.Notices.FirstOrDefault(o => o.SyncItemId == guid && o.CompanyId == companyId);
            //    dbEntity.InitEntity(temp, false);
            //    db.SaveChanges();
            //    var newEntity = db.Notices.FirstOrDefault(o => o.SyncItemId == guid);
            //    return newEntity.SyncItemVersion;
            //}
            // 该表不允许从本地修改
            throw new SyncException("Notice表不允许本地更新");
        }

        public void DeleteItem(Guid syncItemId, int companyId, string storeId)
        {
            //using (var db = SyncDbContextFactory.Factory<EFDbContext>())
            //{
            //    var dbEntity = db.Notices.FirstOrDefault(o => o.SyncItemId == syncItemId && o.CompanyId == companyId);
            //    db.Notices.Remove(dbEntity);
            //    db.SaveChanges();
            //}
            // 该表不允许从本地修改
            throw new SyncException("Notice表不允许本地更新");
        }

        public ISyncDataObject Merge(ISyncDataObject syncDataObject1, ISyncDataObject syncDataObject2, int companyId, string storeId)
        {
            return syncDataObject1;
        }
        public Microsoft.Synchronization.SyncDirectionOrder SyncDirectionOrder
        {
            get { return Microsoft.Synchronization.SyncDirectionOrder.Download; }
        }
    }
}