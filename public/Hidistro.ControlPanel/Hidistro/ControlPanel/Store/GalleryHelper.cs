namespace Hidistro.ControlPanel.Store
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.SqlDal.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class GalleryHelper
    {
        public static bool AddPhote(int categoryId, string photoName, string photoPath, int fileSize)
        {
            return new PhotoGalleryDao().AddPhote(categoryId, photoName, photoPath, fileSize);
        }

        public static bool AddPhotoCategory(string name)
        {
            return new PhotoGalleryDao().AddPhotoCategory(name);
        }

        public static int AddPhotoCategory2(string name)
        {
            return new PhotoGalleryDao().AddPhotoCategory2(name);
        }

        public static bool DeletePhoto(int photoId)
        {
            string photoPath = GetPhotoPath(photoId);
            bool flag = new PhotoGalleryDao().DeletePhoto(photoId);
            if (flag)
            {
                StoreHelper.DeleteImage(photoPath);
            }
            return flag;
        }

        public static bool DeletePhotoCategory(int categoryId)
        {
            return new PhotoGalleryDao().DeletePhotoCategory(categoryId);
        }

        public static int GetDefaultPhotoCount()
        {
            return new PhotoGalleryDao().GetDefaultPhotoCount();
        }

        public static DataTable GetPhotoCategories(int type)
        {
            return new PhotoGalleryDao().GetPhotoCategories(type);
        }

        public static int GetPhotoCount()
        {
            return new PhotoGalleryDao().GetPhotoCount();
        }

        public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, PhotoListOrder order, int type, int pagesize = 20)
        {
            return GetPhotoList(keyword, categoryId, pageIndex, pagesize, order, type);
        }

        public static DbQueryResult GetPhotoList(string keyword, int? categoryId, int pageIndex, int pageSize, PhotoListOrder order, int type)
        {
            Pagination page = new Pagination {
                PageSize = pageSize,
                PageIndex = pageIndex,
                IsCount = true
            };
            switch (order)
            {
                case PhotoListOrder.UploadTimeDesc:
                    page.SortBy = "UploadTime";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UploadTimeAsc:
                    page.SortBy = "UploadTime";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.NameAsc:
                    page.SortBy = "PhotoName";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.NameDesc:
                    page.SortBy = "PhotoName";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UpdateTimeDesc:
                    page.SortBy = "LastUpdateTime";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.UpdateTimeAsc:
                    page.SortBy = "LastUpdateTime";
                    page.SortOrder = SortAction.Asc;
                    break;

                case PhotoListOrder.SizeDesc:
                    page.SortBy = "FileSize";
                    page.SortOrder = SortAction.Desc;
                    break;

                case PhotoListOrder.SizeAsc:
                    page.SortBy = "FileSize";
                    page.SortOrder = SortAction.Asc;
                    break;
            }
            return new PhotoGalleryDao().GetPhotoList(keyword, categoryId, page, type);
        }

        public static string GetPhotoPath(int photoId)
        {
            return new PhotoGalleryDao().GetPhotoPath(photoId);
        }

        public static int MovePhotoType(List<int> pList, int pTypeId)
        {
            return new PhotoGalleryDao().MovePhotoType(pList, pTypeId);
        }

        public static void RenamePhoto(int photoId, string newName)
        {
            new PhotoGalleryDao().RenamePhoto(photoId, newName);
        }

        public static void ReplacePhoto(int photoId, int fileSize)
        {
            new PhotoGalleryDao().ReplacePhoto(photoId, fileSize);
        }

        public static void SwapSequence(int categoryId1, int categoryId2)
        {
            new PhotoGalleryDao().SwapSequence(categoryId1, categoryId2);
        }

        public static int UpdatePhotoCategories(Dictionary<int, string> photoCategorys)
        {
            return new PhotoGalleryDao().UpdatePhotoCategories(photoCategorys);
        }
    }
}

