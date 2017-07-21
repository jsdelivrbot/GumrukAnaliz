using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumruk.Web.Models
{
    public class EntityNodeData
    {
        public List<NodeData> NodeDatas { get; set; }
        public List<NodeLink> NodeLinks { get; set; }
    }
    public class NodeData
    {
        public static string    bluegrad = "<script>SetColor();</script>";
        public static string    greengrad = "$(go.Brush, 'Linear', { 0: 'rgb(158, 209, 159)', 1: 'rgb(67, 101, 56)' });";
        public static string    redgrad = "$(go.Brush, 'Linear', { 0: 'rgb(206, 106, 100)', 1: 'rgb(180, 56, 50)' });";
        public static string    yellowgrad = "<script>setColor();</script>";
        public static string    lightgrad = "$(go.Brush, 'Linear', { 1: '#fff', 0: '#fff' });";
       
        public string key { get; set; }

        public string description { get; set; }
        public List<NodeDataSubitems> items { get; set; }

        public List<NodeDataTable> tables { get; set; }
    }

    public class NodeDataSubitems
    {
        public NodeDataSubitems(string _name,bool _iskey,string _figure,string _color)
        {
            name = _name;
            iskey = iskey;
            figure = _figure;
            color = _color;
        }
        public string name { get; set; }

        public bool iskey { get; set; }

        public string figure { get; set; }

        public string color { get; set; }

    }

    public class NodeDataTable
    {
        public NodeDataTable(string _name, bool _iskey, string _datatype, string _uzunluk, string _description)
        {
            name = _name;
            iskey = _iskey;
            datatype = _datatype;
            uzunluk = _uzunluk;
            description = _description;
        }
        public string name { get; set; }

        public bool iskey { get; set; }

        public string datatype { get; set; }

        public string uzunluk { get; set; }

        public string description { get; set; }
    }


    public class NodeLink
    {
        public NodeLink(string _from, string _to, string _text, string _toText)
        {
            from = _from;
            to = _to;
            text = _text;
            toText = _toText;
        }
        public string from { get; set; }
        public string to { get; set; }
        public string text { get; set; }

        public string toText { get; set; }
    }


}