namespace Hishop.Weixin.MP.Util
{
    using Hishop.Weixin.MP.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    public static class EntityHelper
    {
        public static void FillEntityWithXml<T>(T entity, XDocument doc) where T : AbstractRequest, new()
        {
            T obj = entity;
            if ((object)obj == null)
                obj = Activator.CreateInstance<T>();
            entity = obj;
            XElement root = doc.Root;
            foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
            {
                string name = propertyInfo.Name;
                if (root.Element((XName)name) != null)
                {
                    switch (propertyInfo.PropertyType.Name)
                    {
                        case "DateTime":
                            propertyInfo.SetValue((object)entity, (object)new DateTime(long.Parse(root.Element((XName)name).Value)), (object[])null);
                            break;
                        case "Boolean":
                            if (name == "FuncFlag")
                            {
                                propertyInfo.SetValue((object)entity, (object)(bool)(root.Element((XName)name).Value == "1" ? true : false), (object[])null);
                                break;
                            }
                            else
                                goto default;
                        case "Int64":
                            propertyInfo.SetValue((object)entity, (object)long.Parse(root.Element((XName)name).Value), (object[])null);
                            break;
                        case "Int32":
                            propertyInfo.SetValue((object)entity, (object)int.Parse(root.Element((XName)name).Value), (object[])null);
                            break;
                        case "RequestEventType":
                            propertyInfo.SetValue((object)entity, (object)EventTypeHelper.GetEventType(root.Element((XName)name).Value), (object[])null);
                            break;
                        case "RequestMsgType":
                            propertyInfo.SetValue((object)entity, (object)MsgTypeHelper.GetMsgType(root.Element((XName)name).Value), (object[])null);
                            break;
                        default:
                            propertyInfo.SetValue((object)entity, (object)root.Element((XName)name).Value, (object[])null);
                            break;
                    }
                }
            }
        }

        public static XDocument ConvertEntityToXml<T>(T entity) where T : class, new()
        {
            T obj = entity;
            if ((object)obj == null)
                obj = Activator.CreateInstance<T>();
            entity = obj;
            XDocument xdocument = new XDocument();
            xdocument.Add((object)new XElement((XName)"xml"));
            XElement root = xdocument.Root;
            Func<string, int> orderByPropName = new Func<string, int>(Enumerable.ToList<string>((IEnumerable<string>)new string[13]
            {
        "ToUserName",
        "FromUserName",
        "CreateTime",
        "MsgType",
        "Content",
        "ArticleCount",
        "Articles",
        "FuncFlag",
        "Title ",
        "Description ",
        "PicUrl",
        "Url",
        "Image"
            }).IndexOf);
            foreach (PropertyInfo propertyInfo in Enumerable.ToList<PropertyInfo>((IEnumerable<PropertyInfo>)Enumerable.OrderBy<PropertyInfo, int>((IEnumerable<PropertyInfo>)entity.GetType().GetProperties(), (Func<PropertyInfo, int>)(p => orderByPropName(p.Name)))))
            {
                string name = propertyInfo.Name;
                if (name == "Articles")
                {
                    XElement xelement = new XElement((XName)"Articles");
                    foreach (Article entity1 in propertyInfo.GetValue((object)entity, (object[])null) as List<Article>)
                    {
                        IEnumerable<XElement> enumerable = EntityHelper.ConvertEntityToXml<Article>(entity1).Root.Elements();
                        xelement.Add((object)new XElement((XName)"item", (object)enumerable));
                    }
                    root.Add((object)xelement);
                }
                else if (name == "Image")
                {
                    XElement xelement = new XElement((XName)"Image");
                    xelement.Add((object)new XElement((XName)"MediaId", (object)new XCData(((Image)propertyInfo.GetValue((object)entity, (object[])null)).MediaId)));
                    root.Add((object)xelement);
                }
                else if (name == "")
                {
                    root.Add((object)new XElement((XName)name, (object)new XCData(propertyInfo.GetValue((object)entity, (object[])null) as string ?? "")));
                }
                else
                {
                    switch (propertyInfo.PropertyType.Name)
                    {
                        case "String":
                            root.Add((object)new XElement((XName)name, (object)new XCData(propertyInfo.GetValue((object)entity, (object[])null) as string ?? "")));
                            break;
                        case "DateTime":
                            root.Add((object)new XElement((XName)name, (object)((DateTime)propertyInfo.GetValue((object)entity, (object[])null)).Ticks));
                            break;
                        case "Boolean":
                            if (name == "FuncFlag")
                            {
                                root.Add((object)new XElement((XName)name, (bool)propertyInfo.GetValue((object)entity, (object[])null) ? (object)"1" : (object)"0"));
                                break;
                            }
                            else
                                goto default;
                        case "ResponseMsgType":
                            root.Add((object)new XElement((XName)name, (object)propertyInfo.GetValue((object)entity, (object[])null).ToString().ToLower()));
                            break;
                        case "Article":
                            root.Add((object)new XElement((XName)name, (object)propertyInfo.GetValue((object)entity, (object[])null).ToString().ToLower()));
                            break;
                        default:
                            root.Add((object)new XElement((XName)name, propertyInfo.GetValue((object)entity, (object[])null)));
                            break;
                    }
                }
            }
            return xdocument;
        }
    }
}

