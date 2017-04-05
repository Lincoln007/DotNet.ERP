﻿using Pharos.Logic.OMS.Entity;
using Pharos.Logic.OMS.IDAL;
using Pharos.Utility;
using Pharos.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pharos.Logic.OMS.BLL
{
    public class ProductService : BaseService<ProductRecord>
    {
        #region 私有对象
        [Ninject.Inject]
        IProductRepository ProductRepository { get; set; }
        [Ninject.Inject]
        IBaseRepository<VwProduct> VwProductRepository { get; set; }
        [Ninject.Inject]
        ProductCategoryService ProductCategoryService { get; set; }
        [Ninject.Inject]
        BrandService BrandService { get; set; }
        [Ninject.Inject]
        ImportSetService ImportSetService { get; set; }
        [Ninject.Inject]
        BusinessService BusinessService { get; set; }
        [Ninject.Inject]
        DictionaryService DictionaryService { get; set; }
        [Ninject.Inject]
        IBaseRepository<Traders> TraderRepository { get; set; }
        #endregion
        public Pharos.Utility.OpResult SaveOrUpdate(ProductRecord obj)
        {
            if (!obj.Barcode.IsNullOrEmpty() && ProductRepository.GetQuery(o => o.Barcode == obj.Barcode && o.Id != obj.Id).Any())
                return OpResult.Fail("条码已存在");
            else if(obj.Id==0)
            {
                if (obj.Barcode.IsNullOrEmpty())
                {
                    obj.Barcode =ProductRepository.GenerateNewBarcode(obj.CategorySN);
                }
                obj.CreateUID = CurrentUser.UID;
                obj.CreateDT = DateTime.Now;
                obj.State = 1;
                ProductRepository.Add(obj);
            }
            else
            {
                var source = ProductRepository.Get(obj.Id);
                obj.ToCopyProperty(source, new List<string>() { "CreateUID", "CreateDT", "State","TraderNum" });
                ProductRepository.SaveChanges();
            }
            return OpResult.Success();
        }

        public IEnumerable<dynamic> GetPageList(System.Collections.Specialized.NameValueCollection nvl, out int recordCount)
        {
            var category = nvl["parentType"];//选中分类
            var state = nvl["state"];//状态
            var searchText = nvl["searchText"];//查找文本
            var brandsn = nvl["brandsn"];
            var query = VwProductRepository.GetQuery();
            if (!category.IsNullOrEmpty())
            {
                var bigs = category.Split(',').Select(o => int.Parse(o)).ToList();
                var childs = ProductCategoryService.GetChildSNs(bigs, true);
                query = query.Where(o => childs.Contains(o.CategorySN));
            }
            if (!state.IsNullOrEmpty())
            {
                var ct = short.Parse(state);
                query = query.Where(o => o.State == ct);
            }
            if (!brandsn.IsNullOrEmpty())
            {
                var sn = brandsn.Split(',').Select(o => o.IsNullOrEmpty() ? new Nullable<int>() : o.ToType<int>()).ToList();
                query = query.Where(o => sn.Contains(o.BrandSN));
            }
            if (!searchText.IsNullOrEmpty())
            {
                searchText = searchText.Trim();
                query = query.Where(o => o.Barcode.Contains(searchText) || o.Title.Contains(searchText));
            }
            recordCount = query.Count();
            var list= query.ToPageList();
            var cids = list.Where(o => !o.CompanyIds.IsNullOrEmpty()).Select(o => o.CompanyIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(i => int.Parse(i))).SelectMany(o => o).Distinct().ToList();
            var traders= TraderRepository.GetQuery(o => cids.Contains(o.CID)).ToList();
            return list.Select(o => new { 
                o.Id,
                o.Source,
                o.Barcode,
                o.CategoryTitle,
                o.CategorySN,
                o.BrandTitle,
                o.Title,
                o.Size,
                o.SubUnit,
                o.SysPrice,
                o.TraderNum,
                o.StateTitle,
                CompanyTitle=o.CompanyIds.IsNullOrEmpty()?"":traders.Where(i=>i.CID==int.Parse(o.CompanyIds.Split(',')[0])).Select(i=>i.Title).FirstOrDefault()
            });
        }

        public OpResult Deletes(int[] ids)
        {
            var list= ProductRepository.GetQuery(o => ids.Contains(o.Id)).ToList();
            if (list.Any(o => !o.CompanyIds.IsNullOrEmpty())) return OpResult.Fail("无法移除，该商品存在商户中!");
            ProductRepository.RemoveRange(list);
            return OpResult.Success();
        }
        public OpResult SetState(string ids, short state)
        {
            var sid = ids.Split(',').Select(o => int.Parse(o));
            var list = ProductRepository.GetQuery(o => sid.Contains(o.Id)).ToList();
            list.ForEach(o => o.State = state);
            ProductRepository.SaveChanges();
            return OpResult.Success();
        }

        public ProductRecord GetOne(object id)
        {
            return ProductRepository.Get(id);
        }

        public List<ProductRecord> GetList()
        {
            return ProductRepository.GetQuery().ToList();
        }
        public VwProduct GetProductByBarcode(string barcode)
        {
            if (barcode.IsNullOrEmpty()) return null;
            return VwProductRepository.Find(o => o.Barcode == barcode);
        }
        public OpResult Import(ImportSet obj, System.Web.HttpFileCollectionBase httpFiles, string fieldName, string columnName)
        {
            var op = new OpResult();
            var errLs = new List<string>();
            int count = 0;
            try
            {
                Dictionary<string, char> fieldCols = null;
                DataTable dt = null;
                op = ImportSetService.ImportSet(obj, httpFiles, fieldName, columnName, ref fieldCols, ref dt);
                if (!op.Successed) return op;
                var categorys =ProductCategoryService.GetList();
                var otherClass = categorys.FirstOrDefault(o => o.Title.StartsWith("其"));
                var brands = BrandService.GetList();
                var otherBrand = brands.FirstOrDefault(o => o.Title.StartsWith("其"));
                var brandClass = DictionaryService.GetChildList(5);
                var otherBrandClass = brandClass.FirstOrDefault(o => o.Title.StartsWith("其"));//?
                var units = DictionaryService.GetSubUnitCategories();
                var otherUnit = units.FirstOrDefault(o => o.Title.StartsWith("其"));
                var products = GetList();
                var maxCate = ProductCategoryService.MaxSN()+1;
                var maxBrand = BrandService.MaxSN() + 1;
                var maxDict = DictionaryService.MaxSN() + 1;

                var clsIdx = Convert.ToInt32(fieldCols["CategorySN"]) - 65;
                var brandIdx = Convert.ToInt32(fieldCols["BrandSN"]) - 65;
                var unitIdx = Convert.ToInt32(fieldCols["SubUnitId"]) - 65;
                var titleIdx = Convert.ToInt32(fieldCols["Barcode"]) - 65;
                count = dt.Rows.Count;
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        var dr = dt.Rows[i];
                        #region 条码验证
                        var text = dr[titleIdx].ToString();
                        if (text.IsNullOrEmpty())
                        {
                            errLs.Add("行号[" + i + "]条码为空!");
                            dt.Rows.RemoveAt(i);
                            continue;
                        }
                        else if (products.Any(o => o.Barcode == text))
                        {
                            errLs.Add("条码[" + text + "]已存在!");
                            dt.Rows.RemoveAt(i);
                            continue;
                        }
                        else
                        {
                            products.Add(new ProductRecord() { Barcode = text });
                        }
                        #endregion
                        #region 处理类别
                        text = dr[clsIdx].ToString();
                        if (!text.IsNullOrEmpty() && text.Contains("/"))
                        {
                            var cates = text.Split('/');
                            var first = cates.FirstOrDefault();
                            var third = cates.LastOrDefault();
                            var second = "";
                            if (cates.Length == 3)
                            {
                                second = cates[1];
                                third = cates[2];
                            }
                            else if (cates.Length == 2)
                            {
                                second = cates[1];
                                third = null;
                            }
                            else if (cates.Length > 3)
                            {
                                second = cates[1];
                                third = text.Replace(first + "/" + second + "/", "");
                            }
                            var parent = categorys.FirstOrDefault(o => o.Title == first);

                            var cls = parent != null ? categorys.FirstOrDefault(o => o.CategoryPSN == parent.CategorySN && o.Title == second) : null;
                            cls = cls != null && !third.IsNullOrEmpty() ? categorys.FirstOrDefault(o => o.CategoryPSN == cls.CategorySN && o.Title == third) : cls;
                            if (cls != null)
                            {
                                dr[clsIdx] = cls.CategorySN.ToString();
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    var list = new List<ProductCategory>();
                                    cls = categorys.FirstOrDefault(o => o.Title == first);
                                    int psn = 0;
                                    short grade = 2;
                                    if (cls == null)
                                    {
                                        parent = new ProductCategory()
                                        {
                                            CategorySN = maxCate,
                                            Title = first,
                                            CategoryCode = 1,
                                            Grade = 1,
                                            State = 1,
                                        };
                                        list.Add(parent);
                                        psn = parent.CategorySN;
                                        var child = new ProductCategory()
                                        {
                                            CategoryPSN = psn,
                                            CategorySN = ++maxCate,
                                            Title = second,
                                            CategoryCode = 1,
                                            Grade = grade,
                                            State = 1,
                                        };
                                        list.Add(child);
                                        psn = child.CategorySN;
                                        grade = 3;
                                        if (!third.IsNullOrEmpty())
                                        {
                                            child = new ProductCategory()
                                            {
                                                CategoryPSN = psn,
                                                CategorySN = ++maxCate,
                                                Title = third,
                                                CategoryCode = 1,
                                                Grade = grade,
                                                State = 1,
                                            };
                                            psn = child.CategorySN;
                                            list.Add(child);
                                        }
                                    }
                                    else
                                    {
                                        psn = cls.CategorySN;
                                        cls = categorys.FirstOrDefault(o => o.Title == second);
                                        if (cls == null)
                                        {
                                            var maxcode = ProductCategoryService.MaxCode(psn);
                                            var child = new ProductCategory()
                                            {
                                                CategoryPSN = psn,
                                                CategorySN = ++maxCate,
                                                Title = second,
                                                CategoryCode = ++maxcode,
                                                Grade = grade,
                                                State = 1
                                            };
                                            list.Add(child);
                                            psn = child.CategorySN;
                                            grade = 3;
                                        }
                                        else
                                            psn = cls.CategorySN;
                                        if (!third.IsNullOrEmpty())
                                        {
                                            cls = categorys.FirstOrDefault(o => o.Title == third);
                                            if (cls == null)
                                            {
                                                var maxcode = ProductCategoryService.MaxCode(psn);
                                                var child = new ProductCategory()
                                                {
                                                    CategoryPSN = psn,
                                                    CategorySN = ++maxCate,
                                                    Title = third,
                                                    CategoryCode = ++maxcode,
                                                    Grade = grade,
                                                    State = 1
                                                };
                                                list.Add(child);
                                                psn = child.CategorySN;
                                            }
                                            else
                                                psn = cls.CategorySN;
                                        }
                                    }
                                    ProductCategoryService.AddRange(list);
                                    categorys.AddRange(list);
                                    dr[clsIdx] = psn.ToString();
                                }
                                else if (otherClass != null)
                                {
                                    dr[clsIdx] = otherClass.CategorySN.ToString();
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[titleIdx] + "]类别不存在!");
                                    dt.Rows.RemoveAt(i);//去除不导入
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            errLs.Add("条码[" + dr[titleIdx] + "]类别不存在!");
                            dt.Rows.RemoveAt(i);//去除不导入
                            continue;
                        }
                        #endregion
                        #region 处理品牌
                        text = dr[brandIdx].ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            var cls = brands.FirstOrDefault(o => o.Title == text);
                            if (cls != null)
                            {
                                dr[brandIdx] = cls.BrandSN.ToString();
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    if (otherBrandClass == null)
                                    {
                                        errLs.Add("条码[" + dr[titleIdx] + "]品牌分类不存在!");
                                        dt.Rows.RemoveAt(i);
                                        continue;
                                    }
                                    var data = new ProductBrand()
                                    {
                                        //BrandSN = maxBrand++,
                                        Title = text,
                                        ClassifyId = otherBrandClass.DicSN,
                                        JianPin = "",
                                        State = 1
                                    };
                                    BrandService.SaveOrUpdate(data);
                                    brands.Add(data);
                                    dr[brandIdx] = data.BrandSN.ToString();
                                }
                                else if (otherBrand != null)
                                {
                                    dr[brandIdx] = otherBrand.BrandSN.ToString();
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[titleIdx] + "]品牌不存在!");
                                    dt.Rows.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                        #endregion
                        #region 处理单位
                        text = dr[unitIdx].ToString();
                        if (!text.IsNullOrEmpty())
                        {
                            var cls = units.FirstOrDefault(o => o.Title == text);
                            if (cls != null)
                            {
                                dr[unitIdx] = cls.DicSN.ToString();
                            }
                            else
                            {
                                if (obj.RefCreate)
                                {
                                    var data = new SysDataDictionary()
                                    {
                                        DicPSN = 4,
                                        //DicSN = maxDict++,
                                        Status = true,
                                        Title = text,
                                    };
                                    DictionaryService.SaveOrUpdate(data);
                                    units.Add(data);
                                    dr[unitIdx] = data.DicSN.ToString();
                                }
                                else if (otherUnit != null)
                                {
                                    dr[unitIdx] = otherUnit.DicSN.ToString();
                                }
                                else
                                {
                                    errLs.Add("条码[" + dr[titleIdx] + "]单位不存在!");
                                    dt.Rows.RemoveAt(i);
                                    continue;
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        throw new Exception("创建相关记录失败," + e.Message, e);
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append("begin tran ");
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("insert into ");
                    sb.Append(obj.TableName);
                    sb.Append("(State,");
                    sb.Append(string.Join(",", fieldCols.Keys));
                    sb.Append(") values(1,");
                    foreach (var de in fieldCols)
                    {
                        var index = Convert.ToInt32(de.Value) - 65;
                        try
                        {
                            var text = dr[index].ToString().Trim();
                            sb.Append("'" + text + "',");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("列选择超过范围!", e);
                        }
                    }
                    sb = sb.Remove(sb.Length - 1, 1);
                    sb.Append(");");
                }
                sb.Append(" commit tran");
                if (dt.Rows.Count>0)
                new DBFramework.DBHelper().ExecuteNonQueryText(sb.ToString(), null);
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Successed = false;
                LogEngine.WriteError(ex);
                errLs.Add("导入出现异常!");
            }
            return CommonService.GenerateImportHtml(errLs, count);
        }

        public bool AddProduct(List<VwProduct> products)
        {
            var maxdicsn= DictionaryService.MaxSN();
            var brandClass = DictionaryService.GetChildList(5);
            foreach (var product in products)
            {
                if (!product.Barcode.IsNullOrEmpty() && !product.CategoryTitle.IsNullOrEmpty())
                {
                    var source= ProductRepository.Find(o => o.Barcode == product.Barcode);
                    if (source != null && !(","+source.CompanyIds+",").Contains(","+product.CompanyIds+","))
                    {
                        source.CompanyIds +=","+ product.CompanyIds;
                        ProductRepository.SaveChanges();
                        continue;
                    }
                    var categorys = product.CategoryTitle.Split('/');
                    var first = categorys[0];
                    if (first.IndexOf(']') != -1) first = first.Substring(first.IndexOf(']') + 1);
                    
                    if (!product.BrandTitle.IsNullOrEmpty() && !product.BrandClassTitle.IsNullOrEmpty())
                    {
                        var brandCls = brandClass.FirstOrDefault(o => o.Title == product.BrandClassTitle);
                        if (brandCls == null)
                        {
                            brandCls = new SysDataDictionary()
                            {
                                DicPSN = 5,
                                Status = true,
                                Title = product.BrandClassTitle,
                                DicSN = maxdicsn++,
                                CreateDT = DateTime.Now,
                                CreateUID = CurrentUser.UID
                            };
                            DictionaryService.SaveOrUpdate(brandCls);
                            brandClass.Add(brandCls);
                        }
                        var brand = new ProductBrand() { Source =int.Parse(product.CompanyIds), Title = product.BrandTitle, ClassifyId = brandCls.DicSN };
                        var op = BrandService.SaveOrUpdate(brand);
                        product.BrandSN = brand.BrandSN;
                    }
                    if (!product.SubUnit.IsNullOrEmpty())
                    {
                        var dict = new SysDataDictionary() { DicPSN = 4, Title = product.SubUnit };
                        var op = DictionaryService.SaveOrUpdate(dict);
                        product.SubUnitId = dict.DicSN;
                    }
                    int psn = 0;
                    short g = 1;
                    foreach (var cate in categorys)
                    {
                        if (cate.IsNullOrEmpty()) continue;
                        var ct = cate;
                        if (ct.IndexOf(']') != -1)
                            ct = ct.Substring(ct.IndexOf(']') + 1);
                        var category = new ProductCategory()
                        {
                            CategoryPSN = psn,
                            CategoryCode = ProductCategoryService.MaxCode(psn),
                            Source = 2,
                            Grade = g,
                            Title = ct
                        };
                        var op = ProductCategoryService.SaveOrUpdate(category);
                        psn = category.CategorySN;
                        g++;
                    }
                    product.CategorySN = psn;
                    product.CreateDT = DateTime.Now;
                    product.State = 1;
                    product.Source = 2;
                    var pro = new ProductRecord();
                    product.ToCopyProperty(pro);
                    ProductRepository.Add(pro);
                    
                }
            }
            return true;
        }
    }
}