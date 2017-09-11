namespace Hishop.Weixin.MP.Util
{
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Request;
    using Hishop.Weixin.MP.Request.Event;
    using System;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public static class RequestMessageFactory
    {
        public static AbstractRequest GetRequestEntity(XDocument doc)
        {
            RequestMsgType msgType = MsgTypeHelper.GetMsgType(doc);
            AbstractRequest entity = null;
            switch (msgType)
            {
                case RequestMsgType.Text:
                    entity = new TextRequest();
                    break;

                case RequestMsgType.Image:
                    entity = new ImageRequest();
                    break;

                case RequestMsgType.Voice:
                    entity = new VoiceRequest();
                    break;

                case RequestMsgType.Video:
                    entity = new VideoRequest();
                    break;

                case RequestMsgType.Location:
                    entity = new LocationRequest();
                    break;

                case RequestMsgType.Link:
                    entity = new LinkRequest();
                    break;

                case RequestMsgType.Event:
                    switch (EventTypeHelper.GetEventType(doc))
                    {
                        case RequestEventType.Subscribe:
                            entity = new SubscribeEventRequest();
                            goto Label_00D1;

                        case RequestEventType.UnSubscribe:
                            entity = new UnSubscribeEventRequest();
                            goto Label_00D1;

                        case RequestEventType.Scan:
                            entity = new ScanEventRequest();
                            goto Label_00D1;

                        case RequestEventType.Location:
                            entity = new LocationEventRequest();
                            goto Label_00D1;

                        case RequestEventType.Click:
                            entity = new ClickEventRequest();
                            goto Label_00D1;

                        case RequestEventType.MASSSENDJOBFINISH:
                            entity = new MassendJobFinishEventRequest();
                            goto Label_00D1;
                    }
                    throw new ArgumentOutOfRangeException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        Label_00D1:
            EntityHelper.FillEntityWithXml<AbstractRequest>(entity, doc);
            new Regex(@"<MsgID>(?<url>\d+)</MsgID>");
            if (entity.MsgId == 0L)
            {
                Match match = Regex.Match(doc.Root.ToString(), @"<MsgID>(?<msgid>\d+)</MsgID>", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    entity.MsgId = long.Parse(match.Groups["msgid"].Value);
                    entity.CreateTime = DateTime.Now;
                }
            }
            return entity;
        }
    }
}

