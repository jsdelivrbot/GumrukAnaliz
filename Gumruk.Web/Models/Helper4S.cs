using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Gumruk.Web.Models
{
    public static class Helper4S
    {
        //public static string SiselTextBox<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
        //   Expression<Func<TModel, TValue>> expression)
        //{
        //    var div = new TagBuilder("div");
        //    var lbl = htmlHelper.LabelFor(expression);
        //    var txt = htmlHelper.TextBoxFor(expression);
        //    var error = htmlHelper.ValidationMessageFor(expression);

        //    div.AddCssClass("row");
        //    div.InnerHtml += lbl;
        //    div.InnerHtml += txt;
        //    div.InnerHtml += error;

        //    return div.ToString();
        //}

        //public static string SiselPassWord<TModel, TValue>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TValue>> expression)
        //{
        //    var div = new TagBuilder("div");
        //    var lbl = htmlHelper.LabelFor(expression);
        //    var txt = htmlHelper.PasswordFor(expression);
        //    var error = htmlHelper.ValidationMessageFor(expression);

        //    div.InnerHtml += lbl;
        //    div.InnerHtml += txt;
        //    div.InnerHtml += error;

        //    return div.ToString();
        //}

        const string CLASS = "class";

        //class values
        const string FORM_CONTROL = "form-control";
        const string TEXT_BOX = "text-box";
        const string SINGLE_LINE = "single-line";
        const string CONTROL_LABEL = "col-form-label";
        const string COL_MD_2 = "col-md-2";
        const string COL_MD_10 = "col-md-10";
        const string TEXT_DANGER = "text-danger";
        const string FORM_GROUP = "form-group ";

        //TAG'S
        const string DIV = "div";

        //attributes
        const string FILE = "file";
        const string TYPE = "type";
        public const string FILE_SUFIX = "FileUploader";

        public static List<string> externalForm = new List<string>();
        public static int externalFormCount = 0;


        private static MvcHtmlString putInFormGroup<TModel, TProperty>(
           this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property, HtmlString componente, string DisplayName = "")
        {
            var formGroup = new TagBuilder(DIV);
            formGroup.AddCssClass(FORM_GROUP);

            var attLabel = new RouteValueDictionary();
            attLabel.Add(CLASS, CONTROL_LABEL + " " + COL_MD_2);
            HtmlString label = htmlHelper.LabelFor(property, DisplayName, attLabel);

            var divRight = new TagBuilder(DIV);
            divRight.AddCssClass(COL_MD_10);

            var attValidation = new RouteValueDictionary();
            attValidation.Add(CLASS, TEXT_DANGER);
            HtmlString validation = htmlHelper.ValidationMessageFor(property, "", attValidation);

            divRight.InnerHtml = componente.ToString() + validation.ToString();

            formGroup.InnerHtml = label.ToString() + divRight.ToString();

            return new MvcHtmlString(formGroup.ToString());
        }

        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(
                   this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property, string DisplayName)
        {

            var attTexBox = new RouteValueDictionary();
            attTexBox.Add(CLASS, FORM_CONTROL + " " + TEXT_BOX + " " + SINGLE_LINE);
            HtmlString textbox = htmlHelper.TextBoxFor(property, attTexBox);

            return putInFormGroup(htmlHelper, property, textbox, DisplayName);
        }

        public static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(
                   this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property, string DisplayName)
        {

            var attTexBox = new RouteValueDictionary();
            attTexBox.Add(CLASS, FORM_CONTROL + " " + TEXT_BOX + " " + SINGLE_LINE);
            HtmlString textbox = htmlHelper.TextAreaFor(property, attTexBox);

            return putInFormGroup(htmlHelper, property, textbox, DisplayName);
        }

        public static MvcHtmlString BootstrapFileFor<TModel, TPorperty>(
           this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TPorperty>> property)
        {
            var att = new RouteValueDictionary();
            att.Add(CLASS, FORM_CONTROL);
            att.Add(TYPE, FILE);

            var data = ModelMetadata.FromLambdaExpression(property, htmlHelper.ViewData);
            HtmlString file = htmlHelper.Editor(data.PropertyName + FILE_SUFIX, new { htmlAttributes = att });
            HtmlString hidden = htmlHelper.HiddenFor(property);

            var componente = new HtmlString(file.ToString() + hidden.ToString());

            return putInFormGroup(htmlHelper, property, componente);
        }

        public static MvcHtmlString TextBox4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string datatype = "", string value = "", string placeholder = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<input type='text' name='{0}' id='{0}' class='{1}' {2} {0} {3} placeholder='{4}'/>", name, FORM_CONTROL, required, disabled, placeholder);
            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString Password4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string datatype = "", string value = "", string placeholder = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<input type='password' name='{0}' id='{0}' class='{1}' {2} {0} {3} placeholder='{4}'/>", name, FORM_CONTROL, required, disabled, placeholder);
            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString NumberTextBox4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string datatype = "", string value = "", string placeholder = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<input type='number' name='{0}' id='{0}' class='{1}' {2} {3}  placeholder='{4}'/>", name, FORM_CONTROL, required, disabled, placeholder);
            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString DateTimePicker4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string datatype = "", string value = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<input type='datetime-local' name='{0}' id='{0}' class='{1}' {2} {3}>", name, FORM_CONTROL, required, disabled);
            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString TextArea4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string placeholder = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<textarea id='{0}' name='{0}' class='{1}' {2} {3} placeholder='{4}'/>", name, FORM_CONTROL, required, disabled, placeholder);
            textBox += string.Format("</textarea>");
            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString Modal4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, string source, string windowsize, int lenght = 12, string datatype = "", string key = "", string value = "", string searchURL = "", string placeholder = "")
        {
            string textBox;
            //externalForm = "";
            textBox = string.Format("<div class='form-group row'>");

            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<div>");
            textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            textBox += "</div>";
            textBox += "<div class='input-group'>";
            textBox += string.Format("<input type='text' name='txt{0}' id='txt{0}' class='{1}' {2} {3} onkeypress='return search{0}(event)' placeholder='{4}'>", name, FORM_CONTROL, required, disabled, placeholder);
            textBox += string.Format("<input type='hidden' id='{0}' name='{0}'>", name);
            textBox += string.Format("<span class='input-group-btn'>");
            textBox += string.Format("<a href='javascript:void(0)' id='btn{0}' class='btn btn-default' {1} onclick='ShowModal{0}()'>...</a>", name, disabled);
            textBox += "</span>";
            textBox += "</div></div>";

            textBox += "<script> function search" + name + "(e) {";
            textBox += " if (e.keyCode == 13)";
            textBox += "{";
            textBox += "var search=$('#txt" + name + "').val();console.log(search);";
            textBox += "$.ajax({";
            textBox += "    type: 'POST',";
            textBox += "    url: '" + searchURL + "',";
            textBox += "    data: JSON.stringify({ \"searchValue\": search }),";
            textBox += "    cache: false,";
            textBox += "    contentType: 'application/json; charset=utf-8',";
            textBox += "    success: function myfunction(data) {";
            textBox += string.Format("$('#txt{0}').val(data['{1}']);", name, value);
            textBox += string.Format("$('#{0}').val(data['{1}']);", name, key);
            textBox += " if(data==null) {";
            textBox += string.Format("$('#txt{0}').val('kayıt bulunamadı');", name);
            textBox += "};";
            textBox += "    },";
            textBox += " error: function errorfunction() {";
            textBox += string.Format("$('#txt{0}').val('kayıt bulunamadı');", name);
            textBox += string.Format("$('#{0}').val('0');", name);
            textBox += "},";
            textBox += "    async: false";
            textBox += "})};";
            textBox += "}";
            textBox += "</script>";

            string hiddenName = string.Format("txt{0}", name);
            string controlName = string.Format("{0}", name);

            if (source != "")
            {
                externalForm.Add(string.Format("<div id='modal{0}' class='modal fade' role='dialog'>", name));
                externalForm[externalFormCount] += string.Format("<div class='modal-dialog' id='modalDialog{0}'>", name);
                externalForm[externalFormCount] += "<div class='modal-content " + windowsize + "'>";
                externalForm[externalFormCount] += "<div class='modal-header'>";
                externalForm[externalFormCount] += "<button type='button' class='close' data-dismiss='modal'>&times;</button>";
                externalForm[externalFormCount] += string.Format("<h5 class='modal-title' id='modalHeader{0}'></h5>", name);
                externalForm[externalFormCount] += "</div>";
                externalForm[externalFormCount] += "<div class='modal-body' id='modalBody'>";
                externalForm[externalFormCount] += " " + CreateSubFormFromFile(helper, source, "-fluid", controlName, hiddenName, key, value);
                externalForm[externalFormCount] += "</div>";
                externalForm[externalFormCount] += "</div>";
                externalForm[externalFormCount] += "</div>";
                externalForm[externalFormCount] += "</div>";
                externalForm[externalFormCount] += "<script> function ShowModal" + name + "() {";
                externalForm[externalFormCount] += string.Format("if ($('#txt{0}').val()=='') $('#{0}').val('0');", name);
                externalForm[externalFormCount] += string.Format(" $('#modalHeader{1}').text('{0}');", displayName, name);
                externalForm[externalFormCount] += string.Format(" $('#modal{0}').modal();", name);
                externalForm[externalFormCount] += string.Format("{0}Load();", name);
                externalForm[externalFormCount] += "}</script>";

                if (windowsize != "")
                    externalForm[externalFormCount] += "<style>#modalDialog" + name + "  {width:" + windowsize + "px;}</style>";

                externalFormCount++;
            }

            textBox += "</div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString Collaspse4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, string source, string windowsize, int lenght = 12, string datatype = "", string key = "", string value = "", string searchURL = "")
        {
            string textBox;
            //externalForm = "";
            textBox = string.Format("<div class='form-group row' style='background-color:#ff6600;'>");

            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += "<div class='input-group'>";
            textBox += string.Format("<input type='text' name='txt{0}' id='txt{0}' class='{1}' {2} {3} onkeypress='return search{0}(event)'>", name, FORM_CONTROL, required, disabled);
            textBox += string.Format("<input type='hidden' id='{0}' name='{0}'>", name);
            textBox += string.Format("<span class='input-group-btn'>");
            textBox += string.Format("<a href='javascript:void(0)' id='btn{0}' class='btn btn-default' {1} onclick='Collapse{0}()'>...</a>", name, disabled);
            textBox += "</span>";
            textBox += "</div></div>";

            textBox += "<script> function search" + name + "(e) {";
            textBox += "if($('#txt" + name + "').val()=='') { $('#" + name + "').val('0');};";
            textBox += " if (e.keyCode == 13)";
            textBox += "{";
            textBox += "var search=$('#txt" + name + "').val();console.log(search);";
            textBox += "$.ajax({";
            textBox += "    type: 'POST',";
            textBox += "    url: '" + searchURL + "',";
            textBox += "    data: JSON.stringify({ \"searchValue\": search }),";
            textBox += "    cache: false,";
            textBox += "    contentType: 'application/json; charset=utf-8',";
            textBox += "    success: function myfunction(data) {";
            textBox += string.Format("$('#txt{0}').val(data['{1}']);", name, value);
            textBox += string.Format("$('#{0}').val(data['{1}']);", name, key);
            textBox += " if(data==null) {";
            textBox += string.Format("$('#txt{0}').val('kayıt bulunamadı');", name);
            textBox += "};";
            textBox += "    },";
            textBox += " error: function errorfunction() {";
            textBox += string.Format("$('#txt{0}').val('kayıt bulunamadı');", name);
            textBox += string.Format("$('#{0}').val('0');", name);
            textBox += "},";
            textBox += "    async: false";
            textBox += "})};";
            textBox += "}";
            textBox += "</script>";

            string hiddenName = string.Format("txt{0}", name);
            string controlName = string.Format("{0}", name);

            if (source != "")
            {
                textBox += string.Format("<div id='collapse{0}'>", name);

                textBox += " " + CreateSubFormFromFile(helper, source, "-fluid", controlName, hiddenName, key, value);

                textBox += "</div>";
                textBox += "<script> function Collapse" + name + "() {";

                textBox += string.Format(" $('#collapse{0}').slideToggle('slow');", name);
                textBox += "}</script>";

                //if (windowsize != "")
                //    externalForm[externalFormCount] += "<style>#collapse" + name + "  {width:" + windowsize + "px;}</style>";

                //externalFormCount++;
            }

            textBox += "</div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString Dropdown4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, string master, int lenght = 12, string url = "", string key = "", string value = "", string parameter = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            if (displayName != "")
                textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            textBox += string.Format("<select id='{0}' class='{1}' name='{0}' {2} {3}/>", name, FORM_CONTROL, required, disabled);

            string[] str = url.Split('/');
            url = ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/" + str[0] + "/" + str[1]), false);

            WebClient client = new WebClient();
            dynamic dynJson = "";
            //var content = client.DownloadString(url);
            if (master == "")
                dynJson = JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(client.DownloadData(url)));


            textBox += "<option value=''>Lütfen Seçiniz</option>";
            foreach (var item in dynJson)
            {
                textBox += "<option value='" + item[key] + "'>" + item[value] + "</option>";
            }

            textBox += string.Format("</select>");
            textBox += "</div></div>";

            if (master != "")
            {
                textBox += "<script>$('#" + master + "').change(function(){";
                textBox += "$('#" + name + "').children().remove();";
                textBox += "$('#" + name + "')";
                textBox += ".append($('<option>',  {value: '' }) ";
                textBox += ".text('Lütfen Seçiniz'));";
                textBox += "$.ajax({";
                textBox += "          type: 'GET',";
                textBox += "          url: '" + url + "',";
                textBox += "          data: { " + parameter + " :+$('#" + master + "').val() },";
                textBox += "          cache: false,";
                textBox += "          contenttype: 'application/json; charset=utf-8',";
                textBox += "          success: function myfunction(text,data) {";
                textBox += "            for (i = 0; i < text.length ; i++)";
                textBox += "                {";
                textBox += "                        $('#" + name + "')";
                textBox += "                            .append($('<option>', { value: text[i]['" + key + "'] }) ";
                textBox += "		                    .text(text[i]['" + value + "']));";
                textBox += "                }";
                textBox += "          },";
                textBox += "      async: false";
                textBox += "  });";
                textBox += "});</script>";
            }

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString Submit4S(this HtmlHelper helper, string name, string displayName, string formName, int lenght = 12)
        {
            string btn;

            btn = "<div class='col-lg-6 col-md-6 col-sm-6 col-xs-6' style='padding:1px;'>";
            btn += string.Format("<a href='javascript:void(0)' id='btnsubmit{4}' class='{2} col-lg-12 col-md-12 col-sm-12 col-xs-12{3}'/>{1}</a>", name, displayName, "btn btn-info", "", formName);
            btn += "</div>";
            btn += "<div class='col-lg-6 col-md-6 col-sm-6 col-xs-6' style='padding:1px;'>";
            btn += string.Format("<a href='javascript:void(0)' id='btnReset{4}' class='{2} col-lg-12 col-md-12 col-sm-12 col-xs-12{3}'/>{1}</a>", name, "Temizle", "btn btn-primary", "", formName);
            btn += "</div>";
            btn += "<script>$('#btnsubmit" + formName + "').click(function() {";
            btn += "if (document.getElementById('btnsubmit" + formName + "').disabled==true) {  return false;};";
            btn += string.Format("$('#{0}').validate();", formName);
            btn += "if ($('#" + formName + "').valid()) {";
            btn += string.Format("$('#btnsubmit{0}').text('lütfen beyleyiniz');", formName);

            btn += string.Format("{0}Kaydet();", formName);
            btn += string.Format("$('#btnsubmit{0}').text('{1}');", formName, displayName);

            btn += "}";
            btn += " else { console.log('else geldi')}";
            btn += "});";

            btn += "$('#btnReset" + formName + "').click(function() {";
            btn += string.Format("$('#{0}')[0].reset();", formName);
            btn += string.Format("$('#{1}').find('input[type={0}hidden{0}]').val('0');", "\"", formName);
            btn += string.Format("$('#btnsubmit{0}').removeClass('disabled');", formName);
            btn += "$('#message').hide()";
            btn += "});";

            btn += "</script>";


            return new MvcHtmlString(btn);
        }

        public static MvcHtmlString Button4S(this HtmlHelper helper, string name, string displayName, string formName, int lenght = 12)
        {
            string btn;

            btn = string.Format("<a href='javascript:void(0)' id='btn{0}' class='{2} {3}'/>{1}</a>", name, displayName, "btn btn-default", colSpan(lenght));

            btn += string.Format("$('#btn{0}:enabled').click();", name);

            btn += "<script>$('#btn" + name + "').click(function() {";
            btn += string.Format("$('#{0}').validate();", formName);
            btn += "if ($('#" + formName + "').valid()) {";
            btn += "}});</script>";

            return new MvcHtmlString(btn);
        }

        public static MvcHtmlString Title4S(this HtmlHelper helper, string name, string displayName, int lenght = 12)
        {
            string textBox;
            //textBox = "<div class='well'>";
            textBox = " <div class='title' data-toggle='collapse' data-target='#div" + name + "'>";
            textBox += string.Format("<h4>{0}</h4>", displayName);
            textBox += "</div>";

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString CheckBox4S(this HtmlHelper helper, string name, string displayName, string required, string disabled, int lenght = 12, string datatype = "", string value = "")
        {
            string textBox;

            textBox = string.Format("<div class='form-group row'>");
            textBox += string.Format("<label id='lbl{0}' class='{3} {2}'>{1}</label>", name, displayName, CONTROL_LABEL, colSpan(2));
            textBox += string.Format("<div class='{0}'>", colSpan(lenght));
            //textBox += string.Format("<input type='checkbox' data-toggle='toggle' name='{0}' id='{0}' class='{1}' {2} {3} value='{4}' data-on=' ' data-off=' '/>", name, FORM_CONTROL, required, disabled,value);
            textBox += string.Format("<input type='checkbox'  name='{0}' id='{0}' class='{1}' {2} {3} value='{4}' style='width: 25px; height: 25px;'/>", name, FORM_CONTROL, required, disabled, value);

            textBox += "</div></div>";

            return new MvcHtmlString(textBox);
        }

        public static string colSpan(int lenght)
        {
            //if (lenght == 12)
            //    lenght = 10;

            return string.Format("col-lg-{0} col-md-{0} col-sm-12 col-xs-12", lenght);
        }

        public static MvcHtmlString CreateForm(this HtmlHelper helper, string frm)
        {
            //string opr = "";
            //string items = frm.ToString().Replace("'", "\"");

            //dynamic dynJson = JsonConvert.DeserializeObject(items);
            //opr += "<div class='container'>";
            ////opr += "<div class='well'>";
            //foreach (var item in dynJson)
            //{
            //    if (item.type == "textbox")
            //        opr += TextBox4S(helper, Convert.ToString(item.id), Convert.ToString(item.display), int.Parse(Convert.ToString(item.lenght)));

            //    if (item.type == "title")
            //        opr += Title4S(helper, Convert.ToString(item.id), Convert.ToString(item.display), int.Parse(Convert.ToString(item.lenght)));

            //    if (item.type == "textarea")
            //        opr += TextArea4S(helper, Convert.ToString(item.id), Convert.ToString(item.display), int.Parse(Convert.ToString(item.lenght)));
            //}

            //opr += "</div>";
            ////opr += "</div>";

            MvcHtmlString returnValue = new MvcHtmlString("");
            return returnValue;
        }

        public static MvcHtmlString CreateFormFromFile(this HtmlHelper helper, string fileName, string fluid = "", string displayControl = "", string valueControl = "", string key = "", string value = "")
        {
            string opr = "";
            dynamic dynJson;
            externalForm = new List<string>();
            externalFormCount = 0;
            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/files/" + fileName), false)))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string json = sr.ReadToEnd();
                        dynJson = JObject.Parse(json);
                        //dynJson = JsonConvert.DeserializeObject(json);
                    }
                }
            }



            opr += string.Format("<div class='container{0}'>", fluid);

            if (dynJson["url"] != null && dynJson["form"] != null)
                opr += string.Format("<form  method='post' name='{0}' id='{0}'>", Convert.ToString(dynJson["form"]));

            //opr += "<div class='well'>";
            string ids = "";
            string req = "";
            string disable = "";
            string master = "";
            string buttons = "";
            string parameter = "";
            string source = "";
            string windowsize = "";

            buttons = "<div class='container-row style='margin-bottom:15px;'>";

            if (dynJson["pageTitle"] != null)
                opr += string.Format("<div class='pageTitle'><h3>{0}</h3></div>", Convert.ToString(dynJson["pageTitle"]));

            for (int i = 0; i < dynJson["pageGroup"].Count; i++)
            {

                if (dynJson["pageGroup"][i]["title"] != null)
                {
                    opr += Title4S(helper, Convert.ToString(dynJson["pageGroup"][i]["title"]), Convert.ToString(dynJson["pageGroup"][i]["title"]), 12);
                }
                opr += " <div class='pageGroup' >";
                foreach (var item in dynJson["pageGroup"][i]["items"])
                {
                    if (item["type"] == "textbox" || item["type"] == "textarea" || item["type"] == "dropdown" || item["type"] == "number" || item["type"] == "date" || item["type"] == "modal" || item["type"] == "password")
                        ids += item["id"] + ",";

                    if (item["required"] != null && item["required"] == "true")
                        req = "required";
                    else
                        req = "";

                    if (item["disabled"] != null && item["disabled"] == "true")
                        disable = "disabled";
                    else
                        disable = "";

                    if (item["parameter"] != null)
                        parameter = item["parameter"];
                    else
                        parameter = "";

                    if (item["master"] != null)
                        master = item["master"];
                    else
                        master = "";

                    if (item["source"] != null)
                        source = item["source"];
                    else
                        source = "";


                    if (item["windowsize"] != null)
                        windowsize = item["windowsize"];
                    else
                        windowsize = "";

                    if (item["type"] == "textbox")
                        opr += TextBox4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])), "", "", Convert.ToString(item["placeholder"]));

                    if (item["type"] == "password")
                        opr += Password4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])), "", "", Convert.ToString(item["placeholder"]));

                    if (item["type"] == "textarea")
                        opr += TextArea4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])), Convert.ToString(item["placeholder"]));

                    if (item["type"] == "submit")
                        buttons += Submit4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), Convert.ToString(dynJson["form"]), int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "button")
                        opr += Button4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), Convert.ToString(dynJson["form"]), int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "dropdown")
                        opr += Dropdown4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, master, int.Parse(Convert.ToString(item["lenght"])), Convert.ToString(item["url"]), Convert.ToString(item["key"]), Convert.ToString(item["value"]), parameter);

                    if (item["type"] == "number")
                        opr += NumberTextBox4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])), "", "", Convert.ToString(item["placeholder"]));

                    if (item["type"] == "date")
                        opr += DateTimePicker4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "modal")
                        opr += Modal4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, source, windowsize, int.Parse(Convert.ToString(item["lenght"])), "", Convert.ToString(item["key"]), Convert.ToString(item["value"]), Convert.ToString(item["searchURL"]), Convert.ToString(item["placeholder"]));

                    if (item["type"] == "collapse")
                        opr += Collaspse4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, source, windowsize, int.Parse(Convert.ToString(item["lenght"])), "", Convert.ToString(item["key"]), Convert.ToString(item["value"]), Convert.ToString(item["searchURL"]));

                    if (item["type"] == "checkbox")
                        opr += CheckBox4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])), "", "");
                }

                opr += "</div>";
            }

            opr += "</form>";
            opr += buttons + "</div>";
            //message box ekleniyor.
            opr += "</div>";
            opr += "<div class='container'><div class='alert alert-success'  id='message' style='display:none;'></div></div>";
            if (externalForm.Count() > 0)
            {
                for (int i = 0; i < externalFormCount; i++)
                {
                    opr += externalForm[i];
                }
                externalForm.Clear();
            }

            opr += "</div>";

            opr += "</div>";

            //opr += "</div>";
            opr += CreateSaveFunction(ids, Convert.ToString(dynJson["url"]), Convert.ToString(dynJson["form"]), displayControl, valueControl, key, value, Convert.ToString(dynJson["databaseOBJ"]));

            opr += " <script>jQuery.extend(jQuery.validator.messages, {";
            opr += "required: 'Bu alan zorunlu',";
            opr += "equalTo: 'Girilen şifreler eşlemiyor',";
            opr += "email: 'Geçerli Bir email adresi giriniz',";
            opr += "});</script>";
            MvcHtmlString returnValue = new MvcHtmlString(opr);
            return returnValue;
        }

        public static MvcHtmlString CreateSubFormFromFile(this HtmlHelper helper, string fileName, string fluid = "", string displayControl = "", string valueControl = "", string key = "", string value = "")
        {
            string opr = "";
            dynamic dynJson;

            using (WebClient webClient = new WebClient())
            {
                using (Stream stream = webClient.OpenRead(ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/files/" + fileName), false)))
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string json = sr.ReadToEnd();
                        dynJson = JObject.Parse(json);
                        //dynJson = JsonConvert.DeserializeObject(json);
                    }
                }
            }

            opr += string.Format("<div class='container{0}'>", fluid);


            if (dynJson["url"] != null && dynJson["form"] != null)
                opr += string.Format("<form  method='post' name='{0}' id='{0}'>", Convert.ToString(dynJson["form"]));

            //opr += "<div class='well'>";
            string ids = "";
            string req = "";
            string disable = "";
            string master = "";
            string buttons = "";
            string parameter = "";
            string source = "";
            string windowsize = "";

            if (dynJson["pageTitle"] != null)
                opr += string.Format("<div class='pageTitle'><h3>{0}</h3></div>", Convert.ToString(dynJson["pageTitle"]));

            for (int i = 0; i < dynJson["pageGroup"].Count; i++)
            {

                if (dynJson["pageGroup"][i]["title"] != null)
                {
                    opr += Title4S(helper, Convert.ToString(dynJson["pageGroup"][i]["title"]), Convert.ToString(dynJson["pageGroup"][i]["title"]), 12);
                }
                opr += " <div class='pageGroup' >";
                foreach (var item in dynJson["pageGroup"][i]["items"])
                {
                    if (item["type"] == "textbox" || item["type"] == "textarea" || item["type"] == "dropdown" || item["type"] == "number" || item["type"] == "date" || item["type"] == "modal" || item["type"] == "password")
                        ids += item["id"] + ",";

                    if (item["required"] != null && item["required"] == "true")
                        req = "required";
                    else
                        req = "";

                    if (item["disabled"] != null && item["disabled"] == "true")
                        disable = "disabled";
                    else
                        disable = "";

                    if (item["parameter"] != null)
                        parameter = item["parameter"];
                    else
                        parameter = "";

                    if (item["master"] != null)
                        master = item["master"];
                    else
                        master = "";

                    if (item["source"] != null)
                        source = item["source"];
                    else
                        source = "";


                    if (item["windowsize"] != null)
                        windowsize = item["windowsize"];
                    else
                        windowsize = "";

                    if (item["type"] == "textbox")
                        opr += TextBox4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "password")
                        opr += Password4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "textarea")
                        opr += TextArea4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "submit")
                        buttons += Submit4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), Convert.ToString(dynJson["form"]), int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "button")
                        opr += Button4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), Convert.ToString(dynJson["form"]), int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "dropdown")
                        opr += Dropdown4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, master, int.Parse(Convert.ToString(item["lenght"])), Convert.ToString(item["url"]), Convert.ToString(item["key"]), Convert.ToString(item["value"]), parameter);

                    if (item["type"] == "number")
                        opr += NumberTextBox4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                    if (item["type"] == "date")
                        opr += DateTimePicker4S(helper, Convert.ToString(item["id"]), Convert.ToString(item["display"]), req, disable, int.Parse(Convert.ToString(item["lenght"])));

                }

                opr += "</div>";
            }

            opr += "</form>";
            opr += buttons;

            opr += "</div>";

            //opr += "</div>";
            opr += CreateSaveFunction(ids, Convert.ToString(dynJson["url"]), Convert.ToString(dynJson["form"]), displayControl, valueControl, key, value, Convert.ToString(dynJson["databaseOBJ"]), true);

            MvcHtmlString returnValue = new MvcHtmlString(opr);
            return returnValue;
        }

        //kullanmıyorum. sonra bakarız.
        //public static MvcHtmlString Selector(this HtmlHelper helper, dynamic item)
        //{
        //    MvcHtmlString returnValue = new MvcHtmlString("");
        //    string id = item["id"];
        //    string displayName = item["display"];
        //    string lenght = item["lenght"];
        //    string dataType = "";
        //    string url = "";
        //    string key = "";
        //    string value = "";

        //    if (item["datatype"] != null)
        //        dataType = item["datatype"];

        //    if (item["url"] != null)
        //        url = item["url"];

        //    if (item["key"] != null)
        //        key = item["key"];

        //    if (item["value"] != null)
        //        value = item["value"];

        //    if (item["type"]=="textbox")
        //        returnValue=SiselTextBox(helper, id, displayName,int.Parse(lenght),dataType);

        //    return returnValue;
        //}

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public static string CreateSaveFunction(string ids, string url, string form, string displayControl, string valueControl, string key, string value, string databaseOBJ, bool createLoadMethod = false)
        {
            string[] idArray = ids.Split(',');

            string returnValue = "<script> function " + form + "Kaydet() {";

            if (databaseOBJ == null || databaseOBJ == "")
                returnValue += " var " + form + " = {";
            else
                returnValue += " var " + databaseOBJ + " = {";

            returnValue += string.Format("ID:$('#{0}').val(),", displayControl);
            foreach (var item in idArray)
            {
                if (item != "")
                {
                    returnValue += item + ":$('#" + form + " [id=" + item + "]').is(':checked'),";
                    returnValue += item + ":$('#" + form + " [id=" + item + "]').val(),";
                }
            }


            returnValue += "};";

            if (databaseOBJ == null || databaseOBJ == "")
                returnValue += " console.log( " + form + ");";
            else
                returnValue += " console.log(" + databaseOBJ + ");";

            returnValue += "$.ajax({";
            returnValue += "    type: 'POST',";
            returnValue += "    url: '" + url + "',";
            if (databaseOBJ == null || databaseOBJ == "")
                returnValue += "    data: JSON.stringify(" + form + "),";
            else
                returnValue += "    data: JSON.stringify(" + databaseOBJ + "),";
            returnValue += "    cache: false,";
            returnValue += "    contentType: 'application/json; charset=utf-8',";
            returnValue += "    success: function myfunction(data) {";
            returnValue += "    console.log(data);";
            returnValue += " onSuccess();";
            if (displayControl != "")
            {
                returnValue += "$('#" + valueControl + "').val(data['" + value + "']);";
                //returnValue += "console.log($('#" + valueControl + "').val());";
                returnValue += "$('#" + displayControl + "').val(data['" + key + "']);";
            }

            returnValue += "    },";
            returnValue += "    async: false";
            returnValue += "})};";

            returnValue += " function " + displayControl + "Load() {";
            returnValue += " console.log('" + displayControl + "');";
            returnValue += "console.log($('#" + displayControl + "').val());";
            returnValue += "if($('#" + displayControl + "').val()==0){ console.log('ahanda geldi');$('#" + form + "')[0].reset();return false;}";
            returnValue += "console.log('" + form + "');";
            returnValue += "$.ajax({";
            returnValue += "type: 'POST',";
            returnValue += "url: 'Get" + form + "',";
            returnValue += "data: JSON.stringify( { \"ID\" : $('#" + displayControl + "').val() }),";
            returnValue += "cache: false,";
            returnValue += "contentType: 'application/json; charset=utf-8',";
            returnValue += "success: function myfunction(data) {";
            returnValue += "if (data!=null) {";
            foreach (var item in idArray)
            {
                if (item != "")
                {
                    returnValue += "$('#" + form + " [id=" + item + "]').val(data['" + item + "']);";
                }
            }

            returnValue += "};},";
            returnValue += "async: false});};";

            returnValue += "</script>";


            return returnValue;
        }
    }
}