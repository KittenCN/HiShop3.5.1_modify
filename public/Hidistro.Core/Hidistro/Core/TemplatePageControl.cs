namespace Hidistro.Core
{
    using System;

    public static class TemplatePageControl
    {
        public static int GetPageCount(int totalRecords, int pageSize)
        {
            if ((totalRecords % pageSize) != 0)
            {
                return ((totalRecords / pageSize) + 1);
            }
            return (totalRecords / pageSize);
        }

        public static string GetPageHtml(int pageCount, int pageIndex)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            string str = "<a href='javascript:;' class='prev' href='javascript:void(0);' page='" + (pageIndex - 1) + "'></a>";
            if (pageIndex == 1)
            {
                str = "<a href='javascript:;' class='prev disabled' ></a>";
            }
            string str2 = "";
            if (pageCount > 10)
            {
                str = str + PageNumHtmlMoreThanTen(pageCount, pageIndex);
            }
            else
            {
                str = str + PageNumHtmlLessThanTen(pageCount, pageIndex);
            }
            string str3 = "<a href='javascript:;' class='next' href='javascript:void(0);' page='" + (pageIndex + 1) + "'></a>";
            if (pageIndex == pageCount)
            {
                str3 = "<a href='javascript:;' class='next disabled' ></a>";
            }
            return (str + str2 + str3);
        }

        public static string PageNumHtmlLessThanTen(int pageCount, int pageIndex)
        {
            string str = "";
            for (int i = 1; i <= pageCount; i++)
            {
                if (i == pageIndex)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "<a class='cur' >", i, "</a>" });
                }
                else
                {
                    object obj3 = str;
                    str = string.Concat(new object[] { obj3, "<a href='javascript:void(0);' page='", i, "' >", i, "</a>" });
                }
            }
            return str;
        }

        public static string PageNumHtmlMoreThanTen(int pageCount, int pageIndex)
        {
            string str = "";
            bool flag = true;
            if (pageIndex < 0)
            {
                pageIndex = 1;
            }
            int num = 1;
            if (pageIndex > 10)
            {
                num = pageIndex - 5;
            }
            int num2 = (num + 10) + 1;
            if (num2 >= pageCount)
            {
                num2 = pageCount;
                flag = false;
            }
            int num3 = pageCount;
            if ((num2 + 2) > pageCount)
            {
                num3 = 0;
            }
            for (int i = num; i < num2; i++)
            {
                if (i == pageIndex)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "<a class='cur' >", i, "</a>" });
                }
                else
                {
                    object obj3 = str;
                    str = string.Concat(new object[] { obj3, "<a href='javascript:void(0);' page='", i, "' >", i, "</a>" });
                }
            }
            if (flag)
            {
                object obj4 = str;
                str = string.Concat(new object[] { obj4, "<a href='javascript:void(0);' page='", num2, "' >.....</a>" });
            }
            if (num3 != 0)
            {
                object obj5 = str;
                object obj6 = string.Concat(new object[] { obj5, "<a href='javascript:void(0);' page='", num3 - 2, "' >", num3 - 2, "</a>" });
                str = string.Concat(new object[] { obj6, "<a href='javascript:void(0);' page='", num3 - 1, "' >", num3 - 1, "</a>" });
            }
            return str;
        }
    }
}

