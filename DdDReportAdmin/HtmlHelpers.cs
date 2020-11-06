using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DdDReportAdmin
{
    static public class HTMLHelpers
    {
        public static string RoundedBoxTop(string divInject)
        {
            return RoundedBoxTop(divInject, "");
        }

        public static string RoundedBoxTop(string divInject, string extraClass)
        {
            if (extraClass != null && extraClass.Length > 0)
                extraClass = " " + extraClass;

            return String.Format(@"<div {0} class=""box{1}"">
                                    <img class=""topleft"" src=""images/box-tl.gif""/>
                                    <img class=""bottomleft"" src=""images/box-bl.gif""/>
                                    <img class=""topright"" src=""images/box-tr.gif""/>
                                    <img class=""bottomright"" src=""images/box-br.gif""/>
                                    <table style=""width:100%"" cellspacing=""0"" cellpadding=""0"">
                                    <tr><td class=""top"" colspan=""3""></td></tr>
                                    <tr><td class=""left""><div></div></td>
                                    <td>", divInject, extraClass);
        }

        public static string RoundedBoxBottom()
        {
            return @"</td>
                    <td class=""right""><div></div></td></tr>
                    <tr><td class=""bottom"" colspan=""3""></td></tr>
                    </table>
                    </div>";
        }
    }
}